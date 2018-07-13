using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Model.LiveModels;
using Fanview.API.GraphicsDummyData;

namespace Fanview.API.Repository
{
    public class LiveRepository : ILiveRepository
    {
        private LiveGraphichsDummyData _data;

        public LiveRepository()
        {
            _data = new LiveGraphichsDummyData();
        }
        
        public Task<LiveStatus> GetLiveStatus(string matchId)
        {
            return Task.FromResult(_data.GetDummyLiveStatus());
        }

        public Task<LiveDamageList> GetLiveDamageList(string matchId)
        {
            return Task.FromResult(_data.GetDamagelist());
        }

        public Task<LiveKillList> GetLiveKillList(string matchId)
        {
            return Task.FromResult(_data.GetLiveKillList());
        }

        public Task<LivePlayerStats> GetLivePlayerStats(string matchId)
        {
            return Task.FromResult(_data.GetDummyLiveplayerstats());
        }
    }
}
