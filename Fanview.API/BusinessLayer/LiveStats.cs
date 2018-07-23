using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;
using MongoDB.Driver;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private ILogger<LiveStats> _logger;
        private IPlayerKillRepository _playerKillRepository;
        private ITeamPlayerRepository _teamPlayerRepository;
        private ITeamRepository _teamRepository;
        private IGenericRepository<Event> _EventRepository;
        private IGenericRepository<RankPoints> _rankRepository;
        private IGenericRepository<Event> _tournament;
        private IRanking _ranking;
        private ITeamPlayerRepository _teamPlayerRespository;

        public LiveStats(IPlayerKillRepository playerKillRepository,
                              ITeamPlayerRepository teamPlayerRepository,
                              ITeamRepository teamRepository,
                              IGenericRepository<Event> eventRepository,
                              IGenericRepository<RankPoints> rankRepository,
                              IRanking ranking,
                              IGenericRepository<Event> tournament,
                              ITeamPlayerRepository teamPlayerRespository,
                              ILogger<LiveStats> logger)
        {
            _logger = logger;
            _playerKillRepository = playerKillRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _teamRepository = teamRepository;
            _EventRepository = eventRepository;
            _rankRepository = rankRepository;
            _ranking = ranking;
            _tournament = tournament;
            _teamPlayerRepository = teamPlayerRepository;
        }

        public async Task<IEnumerable<MatchRanking>> GetLiveStatsRanking(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var matchRanking = _ranking.GetTournamentRankings();

            var rankPoints = _rankRepository.GetAll("RankPoints").Result.OrderByDescending(o => o.RankPosition);

            var teamCount = _teamRepository.GetAllTeam().Result.Count();

            var LiveKills = _playerKillRepository.GetLiveKilled(matchId).Result;

            var liveKillCount = _playerKillRepository.GetLiveKillCount(LiveKills);

            var teamEliminationPosition = GetTeamEliminatedPosition(LiveKills, tournamentMatchId, teamCount);

            //var teamEliminatedLastPosition = teamEliminationPosition.TakeLast(1).FirstOrDefault();

            var rankPositionIndex = rankPoints.Select((item, index) => new { Position = (int)index, RankPositionScore = item.ScoringPoints });

            var presentEligibleRankScore = rankPoints.Where(cn => teamEliminationPosition.Select(s => s.Positions).Contains(cn.RankPosition)).Select(s => s.ScoringPoints);



            //rankPositionIndex.Where(cn => teamEliminationPosition.Count() == cn.Position).Select(s => s.RankPositionScore).SingleOrDefault();

            //var teamEliminated = new List<TeamRankPoints>();

            //teamEliminated.Add(new TeamRankPoints() { Name = teamEliminatedLastPosition.Name, TeamId = teamEliminatedLastPosition.TeamId });





            var tournamentRanking = matchRanking.Result.Select(a => new MatchRanking()
            {
                MatchId = a.MatchId,
                TeamId = a.TeamId,
                TeamName = a.TeamName,
                KillPoints = a.KillPoints,
                RankPoints = a.RankPoints,
                TotalPoints = a.TotalPoints,
                TeamRank = a.TeamRank,
                PubGOpenApiTeamId = a.PubGOpenApiTeamId

            });

            var liveRanking = new List<MatchRanking>();
            foreach (var item in tournamentRanking)
            {

                liveRanking.Add(
                new MatchRanking()
                {
                    MatchId = item.MatchId,
                    TeamId = item.TeamId,
                    TeamName = item.TeamName,
                    KillPoints = item.KillPoints,
                    RankPoints = item.RankPoints, //+ presentEligibleRankScore,
                    TotalPoints = item.TotalPoints,
                    TeamRank = item.TeamRank,
                    PubGOpenApiTeamId = item.PubGOpenApiTeamId

                });
            }

            return liveRanking;

        }

        public async Task<LiveStatus> GetLiveStatus(int matchId)
        {

            var teams = _teamRepository.GetAllTeam().Result.AsQueryable();
            

            var teamPlayer = _teamPlayerRepository.GetTeamPlayers().Result.AsQueryable();
           

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result.OrderBy(o => o.EventTimeStamp).AsQueryable();
            

            var playerKilled = kills.Join(teamPlayer, pk => new { VictimTeamId = pk.VictimTeamId }, tp => new { VictimTeamId = tp.TeamIdShort }, (pk, tp) => new { pk, tp })
                                    .Join(teams, pktp => new { TeamShortId = pktp.tp.TeamIdShort }, t => new { TeamShortId = t.TeamId }, (pktp, t) => new { pktp, t })
                                    .Select(s =>
                                        new LiveTeam()
                                        {
                                            Id = s.t.TeamId,
                                            Name = s.t.Name,
                                            TeamPlayers = new LiveTeamPlayers()
                                            {
                                                PlayerName = s.pktp.tp.PlayerName,
                                                PlayeId = s.pktp.tp.PubgAccountId,
                                                PlayerStatus = s.pktp.pk.IsGroggy,
                                                PlayerTeamId = s.pktp.pk.VictimTeamId,
                                                TimeKilled = s.pktp.pk.EventTimeStamp,
                                                location = new LiveLocation()
                                                {
                                                    X = s.pktp.pk.VictimLocation.x,
                                                    Y = s.pktp.pk.VictimLocation.y,
                                                    Z = s.pktp.pk.VictimLocation.z
                                                }
                                            }
                                        }).GroupBy(g => new { PlayerName = g.TeamPlayers.PlayerName, TeamId = g.Id, TeamName = g.Name });


            _logger.LogInformation("PlayerKilled " + playerKilled.Count() + Environment.NewLine);

            var liveStatsStatus = new List<LiveTeamPlayerStatus>();

            foreach (var item in playerKilled)
            {
                var teamPlayers = new List<LiveTeamPlayers>();

                foreach (var item1 in item)
                {
                    teamPlayers.Add(
                        new LiveTeamPlayers()
                        {
                            PlayeId = item1.TeamPlayers.PlayeId,
                            PlayerName = item1.TeamPlayers.PlayerName,
                            PlayerTeamId = item1.TeamPlayers.PlayerTeamId,
                            PlayerStatus = item1.TeamPlayers.PlayerStatus,
                            location = item1.TeamPlayers.location,
                            TimeKilled = item1.TeamPlayers.TimeKilled
                        });
                }

                var teamstats = new LiveTeamPlayerStatus();

                if (liveStatsStatus.Where(cn => cn.Id == item.Key.TeamId).Count() == 0)
                {
                    teamstats.Id = item.Key.TeamId;
                    teamstats.Name = item.Key.TeamName;
                    teamstats.TeamPlayers = teamPlayers.OrderByDescending(o => o.TimeKilled).TakeLast(4);

                    liveStatsStatus.Add(teamstats);
                }
            }

            var liveStatus = new LiveStatus();
            liveStatus.MatchID = matchId;
            liveStatus.teams = liveStatsStatus;

            return await Task.FromResult(liveStatus);
        }


        private IEnumerable<TeamRankPoints> GetTeamEliminatedPosition(IEnumerable<LiveEventKill> kills, string matchId, int totalTeamCount)
        {
            var teamPlayers = new List<PlayerAll>();
            foreach (var item in _teamPlayerRepository.GetTeamPlayers().Result)
            {
                teamPlayers.Add(
                new PlayerAll()
                {
                    longTeamId = item.TeamId,
                    PlayerId = item.PlayerId,
                    PlayerName = item.PlayerName,
                    FullName = item.FullName,
                    Country = item.Country,
                    TeamId = item.TeamIdShort
                });


            }
           

           

            var playersKilled = kills.Join(teamPlayers, pk => pk.VictimTeamId, pc => pc.TeamId,
                                                   (pk, pc) => new { pk, pc })                                                   
                                                   .Select(s => new
                                                   {
                                                       VictimTeamId = s.pk.VictimTeamId,
                                                       pubgTeamId = s.pc.TeamId,
                                                       fanviewTeamId = s.pc.longTeamId,
                                                     //  PlayerAccountId = s.tp.PubgAccountId

                                                   });

            var teamsRankPoints = new List<TeamRankPoints>();

            var teamCount = new List<int>();
            var i = 0;
            foreach (var item in playersKilled)
            {
                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = teamPlayers.Where(cn => cn.TeamId == item.VictimTeamId).Count();

                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamRankFinishing = new TeamRankPoints() { TeamId = item.fanviewTeamId, Positions = GetTeamFinishingPositions(i), OpenApiVictimTeamId = item.VictimTeamId };
                    teamsRankPoints.Add(teamRankFinishing);
                    i++;
                }
            }



            if (teamsRankPoints.Count() < 20 && totalTeamCount < 20)
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


            if (teamsRankPoints.Count() < 20 && teamsRankPoints.Count() != teamPlayers.Select(s => s.TeamId).Distinct().Count())
            {
                var lastTeamStands = kills.Join(teamPlayers, pk => pk.KillerTeamId, tp => tp.TeamId,
                                                   (pk, tp) => new { pk, tp }).Where(cn => !teamsRankPoints.Select(t => t.OpenApiVictimTeamId).Contains(cn.pk.KillerTeamId))
                                                   .Select(r => new TeamRankPoints() { TeamId = r.tp.longTeamId, Positions = 1, OpenApiVictimTeamId = r.pk.KillerTeamId}).GroupBy(g => g.PlayerAccountId).FirstOrDefault().ElementAtOrDefault(0);

                teamsRankPoints.Add(lastTeamStands);

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
