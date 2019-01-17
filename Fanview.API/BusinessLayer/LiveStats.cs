using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;
using MongoDB.Driver;
using Fanview.API.Services.Interface;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private readonly IMatchSummaryRepository _matchSummaryRepository;
        private readonly ITeamLiveStatusRepository _teamLiveStatusRepository;
        private readonly IPlayerKillRepository _playerKillRepository;
        private readonly ITeamRankingService _teamRankingService;
        private readonly IEventRepository _eventRepository;

        public LiveStats(ITeamRepository teamRepository,
                        IMatchSummaryRepository matchSummaryRepository,
                        IEventRepository eventRepository,
                        ITeamLiveStatusRepository teamLiveStatusRepository,
                        IPlayerKillRepository playerKillRepository,
                        ITeamRankingService teamRankingService,
                        ILogger<LiveStats> logger)
        {
            _eventRepository = eventRepository;           
            _matchSummaryRepository = matchSummaryRepository;
            _teamLiveStatusRepository = teamLiveStatusRepository;
            _playerKillRepository = playerKillRepository;
            _teamRankingService = teamRankingService;
        }

        public async Task<EventLiveMatchStatus> GetLiveMatchStatus()
        {
            var liveMatchStatus = await  _teamLiveStatusRepository.GetEventLiveMatchStatus().ConfigureAwait(false);
            liveMatchStatus.MatchState = liveMatchStatus.MatchState.Replace("WaitingPostMatch", "Completed");
            return liveMatchStatus;
        }

        public async Task<IEnumerable<LiveTeamRanking>> GetLiveRanking()
        {
            // Get the match status
            var matchStatus = await _matchSummaryRepository.GetLiveMatchStatusAsync();
            matchStatus = matchStatus.Where(ms => ms.TeamId != 0);

            // Get the live kill list
            var killList = await _playerKillRepository.GetLiveKillListAsync(0);

            return await _teamRankingService.GetTeamRankings(killList, matchStatus);
        } 

        public async Task<IEnumerable<LiveMatchStatus>> GetLiveStatus()
        {
            var matchStatus = await _matchSummaryRepository.GetLiveMatchStatusAsync().ConfigureAwait(false);
            matchStatus.ToList().RemoveAll(m => m.TeamId == 0);
            return matchStatus;
        }

        /**
         * NOT USED: consider them a stub of what have to be implemented
         * */
        //private IEnumerable<TeamRankPoints> GetTeamEliminatedPosition(IEnumerable<LiveEventKill> kills, string matchId, int totalTeamCount)
        //{
        //    var teamPlayers = new List<PlayerAll>();
        //    foreach (var item in _teamPlayerRepository.GetTeamPlayers().Result)
        //    {
        //        teamPlayers.Add(
        //        new PlayerAll()
        //        {
        //            longTeamId = item.TeamId,
        //            PlayerId = item.PlayerId,
        //            PlayerName = item.PlayerName,
        //            FullName = item.FullName,
        //            Country = item.Country,
        //            TeamId = item.TeamIdShort,

        //        });
        //    }

        //    var playersKilled = kills.Select(s => new
        //    {
        //        PlayerAccountId = teamPlayers.Where(cn => cn.PlayerName.Trim().ToLower().Contains(s.VictimName.Trim().ToLower())).FirstOrDefault().PlayerId,
        //        VictimTeamId = s.VictimTeamId,
        //        PlayerName = s.VictimName,
        //        fanviewTeamId = teamPlayers.Where(cn => cn.TeamId == s.VictimTeamId).FirstOrDefault().longTeamId,
        //        OpenApiVictimTeamId = s.VictimTeamId

        //    });

        //    var teamsRankPoints = new List<TeamRankPoints>();

        //    var teamCount = new List<int>();
        //    var i = 0;
        //    foreach (var item in playersKilled)
        //    {
        //        teamCount.Add(item.VictimTeamId);

        //        var teamPlayerCount = teamPlayers.Where(cn => cn.TeamId == item.VictimTeamId).Count();

        //        if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
        //        {
        //            var teamRankFinishing = new TeamRankPoints() { TeamId = item.fanviewTeamId, Positions = GetTeamFinishingPositions(i), OpenApiVictimTeamId = item.VictimTeamId };
        //            teamsRankPoints.Add(teamRankFinishing);
        //            i++;
        //        }
        //    }



        //    if (teamsRankPoints.Count() < 20 && totalTeamCount < 20)
        //    {
        //        var noOfteamEliminated = teamsRankPoints.Count();
        //        var teamDifference = 20 - noOfteamEliminated;

        //        teamsRankPoints = teamsRankPoints.Select(c => new TeamRankPoints()
        //        {
        //            MatchId = c.MatchId,
        //            TeamId = c.TeamId,
        //            Name = c.Name,
        //            OpenApiVictimTeamId = c.OpenApiVictimTeamId,
        //            PlayerAccountId = c.PlayerAccountId,
        //            Positions = c.Positions - teamDifference
        //        }).ToList();
        //    }


        //    if (teamsRankPoints.Count() < 20 && teamsRankPoints.Count() != teamPlayers.Select(s => s.TeamId).Distinct().Count())
        //    {
        //        var lastTeamStands = kills.Join(teamPlayers, pk => pk.KillerTeamId, tp => tp.TeamId,
        //                                           (pk, tp) => new { pk, tp }).Where(cn => !teamsRankPoints.Select(t => t.OpenApiVictimTeamId).Contains(cn.pk.KillerTeamId))
        //                                           .Select(r => new TeamRankPoints() { TeamId = r.tp.longTeamId, Positions = 1, OpenApiVictimTeamId = r.pk.KillerTeamId}).GroupBy(g => g.PlayerAccountId).FirstOrDefault().ElementAtOrDefault(0);

        //        teamsRankPoints.Add(lastTeamStands);

        //    }


        //    return teamsRankPoints;
        //}

        private int GetTeamFinishingPositions(int i)
        {
            var position = 20 - i;
            i++;
            return position;
        }
    }
}
