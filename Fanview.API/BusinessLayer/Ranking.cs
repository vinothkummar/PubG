using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Fanview.API.BusinessLayer
{
    public class Ranking : IRanking
    {
        private ILogger<Ranking> _logger;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private IGenericRepository<RankPoints> _genericRankPointsRepository;
        private IGenericRepository<MatchRanking> _genericMatchRankingRepository;       
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _teamPlayerRespository;
        private IKillingRule _teamKill;        

        public Ranking(ILogger<Ranking> logger, IMatchSummaryRepository matchSummaryRepository, 
                       IPlayerKillRepository playerKillRepository, 
                       IGenericRepository<RankPoints> genericRankPointsRepository,
                       IGenericRepository<MatchRanking> genericMatchRankingRepository,                       
                       ITeamRepository teamRepository,
                       ITeamPlayerRepository teamPlayerRepository, IReadAssets readAssets)
        {
            _logger = logger;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
            _genericRankPointsRepository = genericRankPointsRepository;
            _genericMatchRankingRepository = genericMatchRankingRepository;           
            _teamRepository = teamRepository;
            _teamPlayerRespository = teamPlayerRepository;
            _teamKill = new IndividualPlayerKilled(readAssets);
        }

        public async Task<IEnumerable<MatchRanking>> CalculateMatchRanking(string matchId)
        {
            var kills = _playerKillRepository.GetPlayerKilled(matchId).Result.ToList();

            var matchPlayerStats = _matchSummaryRepository.GetPlayerMatchStats(matchId).Result;

            var teams = _teamRepository.GetTeam().Result.Select(s => new { TeamId = s.Id, TeamName = s.Name });

            var totalTeamCount = teams.Count();
            var teamEliminationPosition = GetTeamEliminatedPosition(kills, matchId, totalTeamCount );

            var rankScorePoints =  _genericRankPointsRepository.GetAll("RankPoints").Result;

            

            var playerKillPointsWithTeam = matchPlayerStats.Select(m => new { m.MatchId, TeamId = m.TeamId, PlayerAccountId = m.stats.PlayerId, KillPoints = m.stats.Kills * 15 })
                                                           .GroupBy(g => new { g.TeamId, g.MatchId })
                                                           .Select(s => new
                                                            { 
                                                                MatchId= s.Key.MatchId,
                                                                TeamId = s.Key.TeamId,
                                                                Killpoints = s.Select(g => g.KillPoints),
                                                                TeamKillTotalPoints = s.Sum(t => t.KillPoints),
                                                                PlayerAccountId = s.Select(g => g.PlayerAccountId)
                                                            });

            

            var teamMatchRankingScrore = playerKillPointsWithTeam.Join(teams, pkp => pkp.TeamId, t => t.TeamId, (pk, t) => new { pk, t })
                                                                 .Join(teamEliminationPosition, tpkp => tpkp.pk.TeamId, tep => tep.TeamId, (tpkp, tep) => new { tpkp, tep})
                                                                 .Join(rankScorePoints, tpktep => tpktep.tep.Positions, rsp => rsp.RankPosition, (tpktep, rsp) => new { tpktep, rsp })
                                                                 .OrderByDescending(o => o.tpktep.tpkp.pk.TeamKillTotalPoints + o.rsp.ScoringPoints)
                                                                 .Select(s => new MatchRanking()
                                                                 {
                                                                     
                                                                     MatchId = s.tpktep.tpkp.pk.MatchId,
                                                                     TeamId = s.tpktep.tpkp.pk.TeamId,
                                                                     TeamName = s.tpktep.tpkp.t.TeamName,
                                                                     KillPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints,
                                                                     RankPoints = s.rsp.ScoringPoints,
                                                                     TotalPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints + s.rsp.ScoringPoints,                                                                     
                                                                     PubGOpenApiTeamId = s.tpktep.tep.OpenApiVictimTeamId
                                                                 });            

            return await Task.FromResult(teamMatchRankingScrore);           
        }

        public async Task<IEnumerable<MatchRanking>> GetMatchRankings(string matchId)
        {
            var matchRankingCollection = _genericMatchRankingRepository.GetMongoDbCollection("MatchRanking");

            var matchRankingScore = matchRankingCollection.FindAsync(Builders<MatchRanking>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            var i = 1;

            var matchStandings = matchRankingScore.Result.Select(s => new MatchRanking()
            {
                MatchId = s.MatchId,
                TeamId = s.TeamId,
                TeamName = s.TeamName,
                KillPoints = s.KillPoints,
                RankPoints = s.RankPoints,
                TotalPoints = s.TotalPoints,
                TeamRank = $"#{i++}",
                PubGOpenApiTeamId = s.PubGOpenApiTeamId
            });

            return await Task.FromResult(matchStandings);

        }

        public Task<IEnumerable<MatchRanking>> GetTournamentRankings()
        {
            var matchRankingCollection = _genericMatchRankingRepository.GetAll("MatchRanking");
            var i = 1;
            var tournamentRankingStandings = matchRankingCollection.Result
                                        .GroupBy(g => g.PubGOpenApiTeamId)
                                        .Select(s => new MatchRanking()
                                        {
                                            PubGOpenApiTeamId = s.Key,                                            
                                            TeamName = s.FirstOrDefault().TeamName,
                                            KillPoints = s.Sum(a => a.KillPoints),
                                            RankPoints = s.Sum(a => a.RankPoints),
                                            TotalPoints = s.Sum(a => a.TotalPoints)
                                        }).OrderByDescending(o => o.TotalPoints).Select(k => new MatchRanking() {
                                            PubGOpenApiTeamId = k.PubGOpenApiTeamId,                                            
                                            TeamName = k.TeamName,
                                            KillPoints = k.KillPoints,
                                            RankPoints = k.RankPoints,
                                            TotalPoints = k.TotalPoints,
                                            TeamRank = $"#{i++}"
                                        });


            return Task.FromResult(tournamentRankingStandings);
          
        }

        public async Task<IEnumerable<DailyMatchRankingScore>> GetSummaryRanking(string matchId1, string matchId2, string matchId3, string matchId4)
        { 
            var kills = _playerKillRepository.GetPlayerKilled(matchId1,matchId2, matchId3, matchId4).Result;

            var matchPlayerStats = _matchSummaryRepository.GetPlayerMatchStats(matchId1, matchId2, matchId3, matchId4).Result;

            var teamEliminationPosition = GetTeamEliminatedPosition(kills, matchId1, matchId2, matchId3, matchId4);

            var rankScorePoints = _genericRankPointsRepository.GetAll("RankPoints").Result;

            var teams = _teamRepository.GetTeam().Result.Select(s => new { TeamId = s.Id, TeamName = s.Name });

            var playerKillPointsWithTeam = matchPlayerStats.Select(m => new {m.MatchId,  TeamId = m.TeamId, PlayerAccountId = m.stats.PlayerId, KillPoints = m.stats.Kills * 15 })
                                                           .GroupBy(g => new {g.MatchId,  g.TeamId })
                                                           .Select(s => new
                                                           {
                                                               s.Key.MatchId,
                                                               s.Key.TeamId,
                                                               Killpoints = s.Select(g => g.KillPoints),
                                                               TeamKillTotalPoints = s.Sum(t => t.KillPoints),
                                                               PlayerAccountId = s.Select(g => g.PlayerAccountId)
                                                           });

        

            var teamMatchRankingScrore = playerKillPointsWithTeam.Join(teams, pkp => pkp.TeamId, t => t.TeamId, (pk, t) => new { pk, t })
                                                                 .Join(teamEliminationPosition, tpkp =>  tpkp.pk.TeamId , tep =>  tep.TeamId, (tpkp, tep) => new { tpkp, tep })
                                                                 .Join(rankScorePoints, tpktep => tpktep.tep.Positions, rsp => rsp.RankPosition, (tpktep, rsp) => new { tpktep, rsp })                                                                                                                             
                                                                 .Select(s => new MatchRanking()
                                                                 {
                                                                     MatchId = s.tpktep.tpkp.pk.MatchId,
                                                                     TeamId = s.tpktep.tpkp.pk.TeamId,
                                                                     TeamName = s.tpktep.tpkp.t.TeamName,
                                                                     KillPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints,
                                                                     RankPoints = s.rsp.ScoringPoints,
                                                                     TotalPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints + s.rsp.ScoringPoints,                                                                     
                                                                     PubGOpenApiTeamId = s.tpktep.tep.OpenApiVictimTeamId
                                                                 }).GroupBy(g => g.MatchId);

            var dailysummaryRanking = new List<DailyMatchRankingScore>();

            foreach (var item in teamMatchRankingScrore)
            {
                var i = 1;

                var dailyMatchRoundRanking = new DailyMatchRankingScore();

                var matchRoundRankingScore = new List<MatchRanking>();
                foreach (var item1 in item.OrderByDescending(o => o.TotalPoints))
                {
                    var matchRankingScore = new MatchRanking() {
                        MatchId = item1.MatchId,
                        TeamId = item1.TeamId,
                        PubGOpenApiTeamId = item1.PubGOpenApiTeamId,
                        TeamName = item1.TeamName,
                        KillPoints = item1.KillPoints,
                        RankPoints = item1.RankPoints,
                        TotalPoints = item1.TotalPoints,
                        TeamRank = $"#{i++}"
                    };

                    matchRoundRankingScore.Add(matchRankingScore);
                }

                dailyMatchRoundRanking.MatchId = item.Key;
                dailyMatchRoundRanking.MatchRankingScores = matchRoundRankingScore;

                dailysummaryRanking.Add(dailyMatchRoundRanking);
            }


            return await Task.FromResult(dailysummaryRanking);
         
        }
        public async Task<IEnumerable<MatchRanking>> PollAndGetMatchRanking(string matchId)
        {
            

                //Task taskA = Task.Factory.StartNew(() =>  _matchSummaryRepository.PollMatchRoundRankingData(matchId));
                await _matchSummaryRepository.PollMatchRoundRankingData(matchId);

                await Task.Delay(3000);

                Task<IEnumerable<MatchRanking>> matchRankings = Task<IEnumerable<MatchRanking>>.Factory.StartNew(() =>
                {
                    var teamsScroingPoints = CalculateMatchRanking(matchId).Result;

                    var matchRankingCollection = _genericMatchRankingRepository.GetMongoDbCollection("MatchRanking");

                    var matchRankingScore = matchRankingCollection.FindAsync(Builders<MatchRanking>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result;

                    if (matchRankingScore == null)
                    {
                    _genericMatchRankingRepository.Insert(teamsScroingPoints, "MatchRanking");
                    }

                    return teamsScroingPoints;
                });

                return await await Task.FromResult(matchRankings);
        }

        private IEnumerable<TeamRankPoints> GetTeamEliminatedPosition(IEnumerable<Kill> kills, string matchId, int totalTeamCount)
        {
            var teamPlayers = _teamPlayerRespository.GetTeamPlayers(matchId).Result;

            //var playersKilled = kills.Join(teamPlayers, pk => pk.Victim.AccountId, tp => tp.PubgAccountId,
            //                                       (pk, tp) => new { pk, tp }).Select(s => new
            //                                       {
            //                                           VictimTeamId = s.pk.Victim.TeamId,
            //                                           TeamId = s.tp.TeamId,
            //                                           PlayerAccountId = s.tp.PubgAccountId

            //                                       });

            var playersCreated = _teamPlayerRespository.GetPlayersCreated(matchId).Result;

            var playersKilled = kills.Join(playersCreated, pk => pk.Victim.AccountId, pc => pc.AccountId,
                                                   (pk, pc) => new { pk, pc })
                                                   .Join(teamPlayers, pkpc => pkpc.pc.AccountId, 
                                                   tp => tp.PubgAccountId, (pkpc, tp) => new {pkpc,tp})
                                                   .Select(s => new
                                                   {
                                                       VictimTeamId = s.pkpc.pk.Victim.TeamId,
                                                       pubgTeamId = s.pkpc.pc.TeamId,
                                                       fanviewTeamId = s.tp.TeamId,
                                                       PlayerAccountId = s.tp.PubgAccountId

                                                   });

            var teamsRankPoints = new List<TeamRankPoints>();

            var teamCount = new List<int>();
            var i = 0;
            foreach (var item in playersKilled)
            {
                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playersCreated.Where(cn => cn.TeamId == item.VictimTeamId.ToString()).Count();

                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamRankFinishing = new TeamRankPoints() { TeamId = item.fanviewTeamId, Positions = GetTeamFinishingPositions(i), OpenApiVictimTeamId = item.VictimTeamId, PlayerAccountId = item.PlayerAccountId };
                    teamsRankPoints.Add(teamRankFinishing);
                    i++;
                }
            }

            

            if (teamsRankPoints.Count()< 20 && totalTeamCount < 20)
            {
                var noOfteamEliminated = teamsRankPoints.Count();
                var teamDifference = 20 - noOfteamEliminated;

                teamsRankPoints = teamsRankPoints.Select(c => new TeamRankPoints()
                {
                    MatchId = c.MatchId,
                    TeamId = c.TeamId,
                    Name = c.Name,
                    OpenApiVictimTeamId = c.OpenApiVictimTeamId,
                    PlayerAccountId = c.PlayerAccountId,
                    Positions = c.Positions - teamDifference
                }).ToList();
            }


            if (teamsRankPoints.Count() < 20 && teamsRankPoints.Count()!= playersCreated.Select(s => s.TeamId).Distinct().Count())
            {
                var lastTeamStands = kills.Join(teamPlayers, pk => pk.Killer.AccountId, tp => tp.PubgAccountId,
                                                   (pk, tp) => new { pk, tp }).Where(cn => !teamsRankPoints.Select(t => t.OpenApiVictimTeamId).Contains(cn.pk.Killer.TeamId))
                                                   .Select(r => new TeamRankPoints() { TeamId = r.tp.TeamId, Positions = 1, OpenApiVictimTeamId = r.pk.Killer.TeamId, PlayerAccountId = r.pk.Killer.AccountId }).GroupBy(g => g.PlayerAccountId).FirstOrDefault().ElementAtOrDefault(0);

                teamsRankPoints.Add(lastTeamStands);

            }


            return teamsRankPoints;
        }

        private IEnumerable<TeamRankPoints> GetTeamEliminatedPosition(IEnumerable<Kill> kills, string matchId1, string matchId2, string matchId3, string matchId4)
        {
            var teamPlayers = _teamPlayerRespository.GetTeamPlayers(matchId1, matchId2, matchId3, matchId4).Result;

            var playersKilled = kills.Join(teamPlayers, pk => new { AccountId = pk.Victim.AccountId, MatchId = pk.MatchId }, tp => new { AccountId = tp.PubgAccountId, MatchId = tp.MatchId }, (pk, tp) => new { pk, tp })
                                     .Select(s => new
                                     {
                                         VictimTeamId = s.pk.Victim.TeamId,
                                         s.tp.TeamId,
                                         KillMatchId = s.pk.MatchId,
                                         TeamEliminatedMatchId = s.tp.MatchId
                                     }).GroupBy(g => g.TeamEliminatedMatchId);

            var teamsRankPoints = new List<TeamRankPoints>();

            var teamRoundElemination = new List<TeamRoundElemination>();
            var i = 0;

            foreach (var item in playersKilled)
            {
                foreach (var item1 in item)
                {
                    var teamRoundPosition = new TeamRoundElemination()
                    {
                        TeamId = item1.VictimTeamId,
                        MatchId = item1.TeamEliminatedMatchId
                    };

                    teamRoundElemination.Add(teamRoundPosition);


                    var teamPlayerCount = kills.Where(cn => cn.Victim.TeamId == item1.VictimTeamId && cn.MatchId == item.Key).Count();


                    if (teamRoundElemination.Where(cn => cn.TeamId == item1.VictimTeamId && cn.MatchId == item.Key).Count() == teamPlayerCount)
                    {
                        var teamRankFinishing = new TeamRankPoints() { TeamId = item1.TeamId, Positions = GetTeamFinishingPositions(i), OpenApiVictimTeamId = item1.VictimTeamId, MatchId = item.Key };
                        teamsRankPoints.Add(teamRankFinishing);
                        i++;
                    }
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
