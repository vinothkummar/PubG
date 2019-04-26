using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;
using Fanview.API.Services.Interface;
using System.Linq;
using System;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private readonly IRanking _ranking;
        private readonly IMatchSummaryRepository _matchSummaryRepository;
        private readonly ITeamLiveStatusRepository _teamLiveStatusRepository;
        private readonly IPlayerKillRepository _playerKillRepository;
        private readonly ITeamRankingService _teamRankingService;

        public LiveStats(IMatchSummaryRepository matchSummaryRepository,
                        ITeamLiveStatusRepository teamLiveStatusRepository,
                        IPlayerKillRepository playerKillRepository,
                        ITeamRankingService teamRankingService,
                        IRanking ranking)
        {
            _matchSummaryRepository = matchSummaryRepository;
            _teamLiveStatusRepository = teamLiveStatusRepository;
            _playerKillRepository = playerKillRepository;
            _teamRankingService = teamRankingService;
            _ranking = ranking;
        }

        public async Task<EventLiveMatchStatus> GetLiveMatchStatus()
        {
            var liveMatchStatus = await  _teamLiveStatusRepository.GetEventLiveMatchStatus().ConfigureAwait(false);
            liveMatchStatus.MatchState = liveMatchStatus.MatchState
                .Replace("WaitingPostMatch", "Completed")
                .Replace("InProgress", "Running")
                .Replace("WaitingToStart", "Waiting");
            return liveMatchStatus;
        }

        public async Task<IEnumerable<LiveTeamRanking>> GetLiveRanking()
        {
            var matchStatusTask = GetLiveStatus();
            var killListTask = _playerKillRepository.GetLiveKillList(64);
            await Task.WhenAll(matchStatusTask, killListTask).ConfigureAwait(false);
            return await _teamRankingService.GetTeamRankings(killListTask.Result, matchStatusTask.Result).ConfigureAwait(false);
        } 

        public Task<IEnumerable<LiveMatchStatus>> GetLiveStatus()
        {
            return _matchSummaryRepository.GetLiveMatchStatus();
        }
        public async Task<List<LiveTeamRanking>> TotalRank()
        {
            var TournamentRank = _ranking.GetTournamentRankings().Result.ToList();
            var TournomentLive = TournamentRank.Select(s => new LiveTeamRanking
            {
                TeamId=s.TeamId,
                TeamName=s.TeamName,
                TeamRank=Convert.ToInt32(s.TeamRank),
                TotalPoints=s.TotalPoints,
                KillPoints=s.KillPoints,
                RankPoints=s.RankPoints
            });
            var LiveRank = this.GetLiveRanking().Result.OrderByDescending(x=>x.TotalPoints).ToList();
            var TotalRank = TournamentRank.Join(LiveRank, innerKey => innerKey.TeamId, outerkey => outerkey.TeamId, (tournamentrak, liverank) =>
              new LiveTeamRanking()
              {
                  TeamId = liverank.TeamId,
                  TeamName = tournamentrak.TeamName,
                  TotalPoints = tournamentrak.TotalPoints + liverank.TotalPoints,
                  KillPoints = tournamentrak.KillPoints + liverank.KillPoints,
                  RankPoints=tournamentrak.RankPoints+liverank.RankPoints,
                  
                  
                  
              }).OrderByDescending(x=>x.TotalPoints).ToList();
            var NewTotal = TotalRank.Select(s => new LiveTeamRanking()
            {
                TeamId = s.TeamId,
                TeamName = s.TeamName,
                KillPoints = s.KillPoints,
                TotalPoints = s.TotalPoints,
                RankPoints = s.RankPoints,
                TeamRank = TotalRank.FindIndex(a => a.TotalPoints == s.TotalPoints) + 1
            }).ToList();
            var combined = TournomentLive.Union(NewTotal).OrderByDescending(x => x.TotalPoints).
                 GroupBy(x => x.TeamId).Select(g=>g.First()).ToList();
             
            return await Task.FromResult(combined);
        }

    }
}
