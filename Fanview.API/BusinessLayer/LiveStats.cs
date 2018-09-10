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

        public async Task<IEnumerable<LiveTeamPlayerStatus>> GetLiveStatus(int matchId)
        {

            var teams = _teamRepository.GetAllTeam().Result.AsQueryable();
            

            var teamPlayer = _teamPlayerRepository.GetTeamPlayers().Result.AsQueryable();
           

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result.OrderByDescending(o => o.EventTimeStamp).AsQueryable();

            
            var playerKilled = kills.Join(teams, pkt => new { TeamId = pkt.VictimTeamId }, t => new { TeamId = t.TeamId }, (pkt, t) => new { pkt, t })
                                    .GroupJoin(teamPlayer, pktp => pktp.pkt.VictimName, tp => tp.PlayerName, (pktp, tp) => new { pktp, tp })
                                    .GroupBy(g => g.pktp.t.TeamId);

            _logger.LogInformation("PlayerKilled " + playerKilled.Count() + Environment.NewLine);

            var liveStatsStatus = new List<LiveTeamPlayerStatus>();

            foreach (var item in playerKilled)
            {
              
                    var team = new LiveTeamPlayerStatus();

                    var teamPlayerStatus = new List<LiveTeamPlayers>();

                    team.Id = item.Select(a => a.pktp.t.TeamId).ElementAtOrDefault(0);
                    team.Name = item.Select(a => a.pktp.t.TeamName).ElementAtOrDefault(0);

                foreach (var item1 in item)
                {
                    var playerStatus = new LiveTeamPlayers();

                    var teamplayerExists = teamPlayerStatus.Where(cn => cn.PlayerName == item1.pktp.pkt.VictimName).Count();

                    if (teamplayerExists == 0)
                    {
                        playerStatus.PlayerId = teamPlayer.Where(cn => cn.PlayerName == item1.pktp.pkt.VictimName).Select(s => s.PlayerId).ElementAtOrDefault(0);
                        playerStatus.PlayerName = item1.pktp.pkt.VictimName;
                        playerStatus.PlayerStatus = item1.pktp.pkt.IsGroggy;
                        playerStatus.TimeKilled = item1.pktp.pkt.EventTimeStamp;

                        teamPlayerStatus.Add(playerStatus);
                        team.TeamPlayers = teamPlayerStatus;
                    }
                    
                }

                    liveStatsStatus.Add(team);

            }

            var t1 = liveStatsStatus.Select(s => s.TeamPlayers).ToList();

            var t2 = teamPlayer.ToList();



            foreach (var item in t2)
            {
                if (item.PlayerName != null)
                {
                    var team = new LiveTeamPlayerStatus();

                    team.Id = item.TeamIdShort;
                    var teamplayerExists1 = liveStatsStatus.Where(cn => cn.TeamPlayers.Select(k => k.PlayerName.Trim()).Contains(item.PlayerName.Trim())).Count();

                    if (teamplayerExists1 == 0)
                    {
                        var playerinfo = new LiveTeamPlayers();
                        playerinfo.PlayerId = item.PlayerId;
                        playerinfo.PlayerName = item.PlayerName;
                        playerinfo.PlayerStatus = true;

                        liveStatsStatus.Add(team);
                    }


                }
            }


            var liveStatus = new LiveStatus();
            liveStatus.MatchID = matchId;
            liveStatus.teams = liveStatsStatus;



           

            

           return await Task.FromResult(liveStatsStatus);
            //return null;
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
                    TeamId = item.TeamIdShort,
                    
                });


            }

            var playersKilled = kills.Select(s => new
            {
                PlayerAccountId = teamPlayers.Where(cn => cn.PlayerName.Trim().ToLower().Contains(s.VictimName.Trim().ToLower())).FirstOrDefault().PlayerId,
                VictimTeamId = s.VictimTeamId,
                PlayerName = s.VictimName,
                fanviewTeamId = teamPlayers.Where(cn => cn.TeamId == s.VictimTeamId).FirstOrDefault().longTeamId,
                OpenApiVictimTeamId = s.VictimTeamId

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
