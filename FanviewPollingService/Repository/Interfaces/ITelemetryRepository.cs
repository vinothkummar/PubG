using System;
using System.Collections.Generic;
using System.Text;

namespace FanviewPollingService.Repository.Interfaces
{
    public interface ITelemetryRepository
    {
        void InsertPlayerKillTelemetry(string jsonResult);

        void GetPlayerKillTelemetryJson();
       
    }
}
