using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerRepository
    {
        void InsertVehicleLeaveTelemetry(string jsonResult);

        void InsertLogPlayerPosition(string jsonResult, string matchId);

        Task<IEnumerable<VehicleLeave>> GetPlayerLeftVechile();
    }
}
