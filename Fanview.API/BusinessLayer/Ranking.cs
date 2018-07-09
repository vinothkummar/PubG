using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Repository;
using Microsoft.Extensions.Logging;

namespace Fanview.API.BusinessLayer
{
    public class Ranking : IRanking
    {
        private ILogger<Ranking> _logger;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private IGenericRepository<RankPoints> _genericRankPointsRepository;
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _teamPlayerRespository;
        private IKillingRule _teamKill;        

        public Ranking(ILogger<Ranking> logger, IMatchSummaryRepository matchSummaryRepository, 
                       IPlayerKillRepository playerKillRepository, 
                       IGenericRepository<RankPoints> genericRankPointsRepository,
                       ITeamRepository teamRepository,
                       ITeamPlayerRepository teamPlayerRepository, IReadAssets readAssets)
        {
            _logger = logger;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
            _genericRankPointsRepository = genericRankPointsRepository;
            _teamRepository = teamRepository;
            _teamPlayerRespository = teamPlayerRepository;
            _teamKill = new IndividualPlayerKilled(readAssets);
        }

        public async Task<IEnumerable<MatchRanking>> GetMatchRanking(string matchId)
        {
            var kills = _playerKillRepository.GetPlayerKilled(matchId).Result;

            var matchPlayerStats = _matchSummaryRepository.GetPlayerMatchStats(matchId).Result;

            var teamEliminationPosition = GetTeamEliminatedPosition(kills, matchId);

            var rankScorePoints =  _genericRankPointsRepository.GetAll("RankPoints").Result;

            var teams = _teamRepository.GetTeam().Result.Select(s => new { TeamId = s.Id, TeamName = s.Name });

            var playerKillPointsWithTeam = matchPlayerStats.Select(m => new { TeamId = m.TeamId, PlayerAccountId = m.stats.PlayerId, KillPoints = m.stats.Kills * 15 })
                                                           .GroupBy(g => new { g.TeamId })
                                                           .Select(s => new
                                                            {
                                                                TeamId = s.Key.TeamId,
                                                                Killpoints = s.Select(g => g.KillPoints),
                                                                TeamKillTotalPoints = s.Sum(t => t.KillPoints),
                                                                PlayerAccountId = s.Select(g => g.PlayerAccountId)
                                                            });

            var i = 1;

            var teamMatchRankingScrore = playerKillPointsWithTeam.Join(teams, pkp => pkp.TeamId, t => t.TeamId, (pk, t) => new { pk, t })
                                                                 .Join(teamEliminationPosition, tpkp => tpkp.pk.TeamId, tep => tep.TeamId, (tpkp, tep) => new { tpkp, tep })
                                                                 .Join(rankScorePoints, tpktep => tpktep.tep.Positions, rsp => rsp.RankPosition, (tpktep, rsp) => new { tpktep, rsp })
                                                                 .OrderByDescending(o => o.tpktep.tpkp.pk.TeamKillTotalPoints + o.rsp.ScoringPoints)
                                                                 .Select(s => new MatchRanking()
                                                                 {
                                                                     MatchId = s.tpktep.tpkp.pk.TeamId,
                                                                     TeamName = s.tpktep.tpkp.t.TeamName,
                                                                     KillPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints,
                                                                     RankPoints = s.rsp.ScoringPoints,
                                                                     TotalPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints + s.rsp.ScoringPoints,
                                                                     TeamRank = $"#{i++}"
                                                                 });

            return await Task.FromResult(teamMatchRankingScrore);
        }

        private IEnumerable<TeamRankPoints> GetTeamEliminatedPosition(IEnumerable<Kill> kills, string matchId)
        {
            var teamPlayers = _teamPlayerRespository.GetTeamPlayers(matchId).Result;

            var playersKilled = kills.Join(teamPlayers, pk => pk.Victim.AccountId, tp => tp.PubgAccountId,
                                                   (pk, tp) => new { pk, tp }).Select(s => new {
                                                       VictimTeamId = s.pk.Victim.TeamId,
                                                       TeamId = s.tp.TeamId
                                                   });
            
            var teamsRankPoints = new List<TeamRankPoints>();

            var teamCount = new List<int>();
            var i = 0;
            foreach (var item in playersKilled)
            {
                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = kills.Where(cn => cn.Victim.TeamId == item.VictimTeamId).Count();

                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamRankFinishing = new TeamRankPoints() { TeamId = item.TeamId, Positions = GetTeamFinishingPositions(i), OpenApiVictimTeamId = item.VictimTeamId };
                    teamsRankPoints.Add(teamRankFinishing);
                    i++;
                }
            }

            return teamsRankPoints;
        }

        private int GetTeamFinishingPositions(int i)
        {

            var position = 20 - i;
            i++;
            return position;
        }
    }
}
