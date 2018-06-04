using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using FanviewPollingService.Model;
using FanviewPollingService.Contracts;

namespace Fanview.API.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        public Task<IEnumerable<PlayerKill>> GetPlayerKills()
        {
            throw new NotImplementedException();
        }
    }
}
