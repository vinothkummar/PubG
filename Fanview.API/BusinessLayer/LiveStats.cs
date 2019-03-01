using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;
using Fanview.API.Services.Interface;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private readonly IMatchSummaryRepository _matchSummaryRepository;
        private readonly ITeamLiveStatusRepository _teamLiveStatusRepository;
        private readonly IPlayerKillRepository _playerKillRepository;
        private readonly ITeamRankingService _teamRankingService;

        public LiveStats(IMatchSummaryRepository matchSummaryRepository,
                        ITeamLiveStatusRepository teamLiveStatusRepository,
                        IPlayerKillRepository playerKillRepository,
                        ITeamRankingService teamRankingService)
        {
            _matchSummaryRepository = matchSummaryRepository;
            _teamLiveStatusRepository = teamLiveStatusRepository;
            _playerKillRepository = playerKillRepository;
            _teamRankingService = teamRankingService;
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
    }
}
