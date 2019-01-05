using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using MongoDB.Driver;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;

namespace Fanview.API.Repository
{
    public class TeamLiveStatusRepository : ITeamLiveStatusRepository
    {
        private IGenericRepository<LiveMatchStatus> _liveMatchStatusRepository;
        private IGenericRepository<EventLiveMatchStatus> _eventLiveMatchStatus;
        private ICacheService _cacheService;
        private ILogger<TeamLiveStatusRepository> _logger;

        public TeamLiveStatusRepository(IGenericRepository<LiveMatchStatus> liveMatchStatusRepository, IGenericRepository<EventLiveMatchStatus> eventLiveMatchStatus, ICacheService cacheService,
                                        ILogger<TeamLiveStatusRepository> logger)
        {
            _liveMatchStatusRepository = liveMatchStatusRepository;
            _eventLiveMatchStatus = eventLiveMatchStatus;
            _cacheService = cacheService;
            _logger = logger;
        }

        public void CreateEventLiveMatchStatus(IEnumerable<EventLiveMatchStatus> eventLiveMatchStatus)
        {
            _eventLiveMatchStatus.Insert(eventLiveMatchStatus, "EventMatchStatus");
        }

        public void CreateTeamLiveStatus(IList<LiveMatchStatus> teamLiveStatuses)
        {
            _liveMatchStatusRepository.Insert(teamLiveStatuses, "TeamLiveStatus");
        }

        public async Task<EventLiveMatchStatus> GetEventLiveMatchStatus()
        {
            var eventLiveMatchStatus = _eventLiveMatchStatus.GetMongoDbCollection("EventMatchStatus");

            //this feature is commented ; due to matchId issue on the OGN Test

            //var lastLiveMatchStatus = eventLiveMatchStatus.FindAsync(Builders<EventLiveMatchStatus>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync().Result.OrderByDescending(o => o.Id).FirstOrDefault();
            var lastLiveMatchStatus = eventLiveMatchStatus.FindAsync(Builders<EventLiveMatchStatus>.Filter.Empty).Result.ToListAsync().Result.OrderByDescending(o => o.Id).FirstOrDefault();

            return await Task.FromResult(lastLiveMatchStatus);

        }

        public async Task<int> GetTeamLiveStatusCount(string matchId)
        {
            var cacheKey = "TeamLiveStatusCountCach";

            try
            {
                var TeamLiveStatusCountCache = _cacheService.RetrieveFromCache<int>(cacheKey);

                if (TeamLiveStatusCountCache != 0)
                {
                    return TeamLiveStatusCountCache;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("TeamLiveStatusCountCache exception " + ex + Environment.NewLine);
            }

            var liveMatchStatus = _liveMatchStatusRepository.GetMongoDbCollection("TeamLiveStatus");

            var TeamLiveStatusCount = liveMatchStatus.FindAsync(Builders<LiveMatchStatus>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync().Result.Count;

            return await Task.FromResult(TeamLiveStatusCount);

        }

        public void ReplaceTeamLiveStatus(LiveMatchStatus liveMatchStatus, FilterDefinition<LiveMatchStatus> filter)
        {
            _liveMatchStatusRepository.Replace(liveMatchStatus, filter, "TeamLiveStatus");
        }
    }
}
