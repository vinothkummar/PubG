using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface ITeamRankingService
    {
        Task<IEnumerable<LiveTeamRanking>> GetTeamRankings(KillLeader killLeader, 
            IEnumerable<LiveMatchStatus> liveStatus);
    }
}
