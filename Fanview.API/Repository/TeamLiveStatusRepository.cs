using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class TeamLiveStatusRepository : ITeamLiveStatusRepository
    {
        private IGenericRepository<LiveMatchStatus> _liveMatchStatusRepository;
        private IGenericRepository<EventLiveMatchStatus> _eventLiveMatchStatus;

        public TeamLiveStatusRepository(IGenericRepository<LiveMatchStatus> liveMatchStatusRepository, IGenericRepository<EventLiveMatchStatus> eventLiveMatchStatus)
        {
            _liveMatchStatusRepository = liveMatchStatusRepository;
            _eventLiveMatchStatus = eventLiveMatchStatus;
        }

        public void CreateEventLiveMatchStatus(IEnumerable<EventLiveMatchStatus> eventLiveMatchStatus)
        {
            _eventLiveMatchStatus.Insert(eventLiveMatchStatus, "EventMatchStatus");
        }

        public void CreateTeamLiveStatus(IList<LiveMatchStatus> teamLiveStatuses)
        {
            _liveMatchStatusRepository.Insert(teamLiveStatuses, "TeamLiveStatus");
        }

        public async Task<EventLiveMatchStatus> GetEventLiveMatchStatus(string matchId)
        {
            var eventLiveMatchStatus = _eventLiveMatchStatus.GetMongoDbCollection("EventMatchStatus");

            var lastLiveMatchStatus = eventLiveMatchStatus.FindAsync(Builders<EventLiveMatchStatus>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync().Result.OrderByDescending(o => o.Id).FirstOrDefault();

            return await Task.FromResult(lastLiveMatchStatus);

        }

        public async Task<int> GetTeamLiveStatusCount(string matchId)
        {
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
