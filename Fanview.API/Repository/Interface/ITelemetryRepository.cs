using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FanviewPollingService.Model;

namespace Fanview.API.Repository.Interface
{
    public interface ITelemetryRepository
    {
        Task<IEnumerable<PlayerKill>> GetPlayerKills();
    }
}
