using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerRepository
    {
        void InsertVehicleLeaveTelemetry(string jsonResult, string matchId);

        void InsertParachuteLanding(string jsonResult, string matchId);

        void InsertLogPlayerPosition(string jsonResult, string matchId);

        Task<IEnumerable<VehicleLeave>> GetPlayerLeftVechile();

        Task<FlightPath> GetFlightPath(int matchId);
    }
}
