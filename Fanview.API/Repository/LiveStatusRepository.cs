using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository.Interface;

namespace Fanview.API.Repository
{
    public class LiveStatusRepository : ILiveStatusRepository
    {
        public Task<LiveStatus> GetMatchLiveStatus()
        {
            throw new NotImplementedException();
        }
    }
}
