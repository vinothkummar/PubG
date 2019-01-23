using Fanview.API.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamLiveStatusRepository
    {
        Task<long> GetTeamLiveStatusCount(string matchId);

        void CreateTeamLiveStatus(IList<LiveMatchStatus> teamLiveStatuses);

        void ReplaceTeamLiveStatus(LiveMatchStatus liveMatchStatus, FilterDefinition<LiveMatchStatus> filter);

        void CreateEventLiveMatchStatus(IEnumerable<EventLiveMatchStatus> eventLiveMatchStatus);

        Task<EventLiveMatchStatus> GetEventLiveMatchStatus();

    }
}
