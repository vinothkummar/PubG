using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface ILiveStatusRepository
    {
        Task<LiveStatus> GetMatchLiveStatus();
    }
}
