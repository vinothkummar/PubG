using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface ILiveRepository
    {
        Task<LiveStatus> GetLiveStatus(string matchId);

        Task<LiveDamageList> GetLiveDamageList(string matchId);

        Task<LiveKillList> GetLiveKillList(string matchId);

        Task<LivePlayerStats> GetLivePlayerStats(string matchId);
    }
}
