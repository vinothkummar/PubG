using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface ILiveStats
    {
        Task<IEnumerable<LiveMatchStatus>> GetLiveStatus();
        Task<IEnumerable<LiveTeamRanking>> GetLiveRanking();
        Task<EventLiveMatchStatus> GetLiveMatchStatus();
    }
}