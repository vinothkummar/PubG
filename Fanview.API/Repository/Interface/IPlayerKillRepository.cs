using System;
using System.Collections.Generic;
using System.Text;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerKillRepository
    {
        void InsertPlayerKillTelemetry(string jsonResult);

        void GetPlayerKillTelemetryJson();
       
    }
}
