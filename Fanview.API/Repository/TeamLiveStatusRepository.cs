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
            var eventMatchStatus = _eventLiveMatchStatus.GetMongoDbCollection("EventMatchStatus");
            var filter = Builders<EventLiveMatchStatus>.Filter.Empty;
            var options = new FindOptions<EventLiveMatchStatus>()
            {
                Sort = Builders<EventLiveMatchStatus>.Sort.Descending("_id"),
                Limit = 1
            };
            var res = await eventMatchStatus.FindAsync(filter, options).ConfigureAwait(false);
            return await res.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public Task<long> GetTeamLiveStatusCount(string matchId)
        {
            var collection = _liveMatchStatusRepository.GetMongoDbCollection("TeamLiveStatus");
            var filter = Builders<LiveMatchStatus>.Filter.Where(ms => ms.MatchId == matchId);
            return collection.CountDocumentsAsync(filter);
        }

        public void ReplaceTeamLiveStatus(LiveMatchStatus liveMatchStatus, FilterDefinition<LiveMatchStatus> filter)
        {
            _liveMatchStatusRepository.Replace(liveMatchStatus, filter, "TeamLiveStatus");
        }
    }
}