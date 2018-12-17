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
using System.Globalization;
using Fanview.API.Utility;
using Fanview.API.Services.Interface;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private ILogger<LiveStats> _logger;        
        private ITeamPlayerRepository _teamPlayerRepository;
        private IMatchSummaryRepository _matchSummaryRepository;
        private ITeamLiveStatusRepository _teamLiveStatusRepository;
        private ICacheService _cacheService;
        private ITeamRepository _teamRepository;
        private IRanking _ranking;      
        private IEventRepository _eventRepository;


        public LiveStats(ITeamPlayerRepository teamPlayerRepository,
                              ITeamRepository teamRepository,
                              IRanking ranking,
                              IMatchSummaryRepository matchSummaryRepository,
                              IEventRepository eventRepository,
                              ICacheService cacheService,
                              ITeamLiveStatusRepository teamLiveStatusRepository,
                              ILogger<LiveStats> logger)
        {
            _logger = logger;            
            _teamPlayerRepository = teamPlayerRepository;           
            _teamRepository = teamRepository;
            _eventRepository = eventRepository;           
            _ranking = ranking;                     
            _matchSummaryRepository = matchSummaryRepository;
            _teamLiveStatusRepository = teamLiveStatusRepository;
            _cacheService = cacheService;
        }

        public async Task<object> GetLiveMatchStatus(int matchId)
        {
            var liveMatchStatus = await  _teamLiveStatusRepository.GetEventLiveMatchStatus(await _eventRepository.GetTournamentMatchId(matchId));

            var result = new
            {
                MatchId = matchId,
                MatchState = liveMatchStatus.MatchState == "WaitingPostMatch" ? "Completed" : liveMatchStatus.MatchState,
                ElapsedTime = liveMatchStatus.ElapsedTime,
                BlueZonePhase = liveMatchStatus.BlueZonePhase,
                IsBlueZoneMoving = liveMatchStatus.IsBlueZoneMoving,
                BlueZoneRadius = liveMatchStatus.BlueZoneRadius,
                BlueZoneLocation = liveMatchStatus.BlueZoneLocation,
                WhiteZoneRadius = liveMatchStatus.WhiteZoneRadius,
                WhiteZoneLocation = liveMatchStatus.WhiteZoneLocation,
                RedZoneRadius = liveMatchStatus.RedZoneRadius,
                RedZoneLocation = liveMatchStatus.RedZoneLocation,
                StartPlayerCount = liveMatchStatus.StartPlayerCount,
                AlivePlayerCount = liveMatchStatus.AlivePlayerCount,
                StartTeamCount = liveMatchStatus.StartTeamCount,
                AliveTeamCount = liveMatchStatus.AliveTeamCount
            };

            return result;
        }

        public Task<object> GetLiveRanking(int matchId)
        {
            return null;
        }

        public async Task<Object> GetLiveStatus(int matchId)
        {
            try
            {
                var teamLiveStatusCache = await _cacheService.RetrieveFromCache<Object>("TeamLiveStatusCache");

                if (teamLiveStatusCache != null)
                {
                    return teamLiveStatusCache;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("TeamLiveStatusCache exception " + ex + Environment.NewLine);

            }




            var matchStatus = _matchSummaryRepository.GetLiveMatchStatus(matchId).Result;

            object matchStatusObject = matchStatus.Where(cn => cn.TeamId != 0).Select(s => new 
                                    {
                                        TeamId = s.TeamId,
                                        TeamName = s.TeamName,
                                        Players = s.TeamPlayers,
                                        AliveCount = s.AliveCount,
                                        DeadCount = s.DeadCount,
                                        EliminatedAt = s.EliminatedAt,
                                        IsEliminated = s.IsEliminated
                                    });

            await _cacheService.SaveToCache<Object>("TeamLiveStatusCache", matchStatusObject, 30, 10);


            if (matchStatusObject == null)
            {
                return null;
            }

            

            return await Task.FromResult(matchStatusObject);            
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
