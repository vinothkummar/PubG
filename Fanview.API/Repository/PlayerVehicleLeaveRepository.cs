using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository
{
    public class PlayerVehicleLeaveRepository : IPlayerVehicleLeaveRepository
    {
        private IGenericRepository<VehicleLeave> _genericRepository;
        private ILogger<PlayerVehicleLeaveRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime LastVehicleLeaveTimeStamp = DateTime.MinValue;

        public PlayerVehicleLeaveRepository(IGenericRepository<VehicleLeave> genericRepository, ILogger<PlayerVehicleLeaveRepository> logger)
        {
            _genericRepository = genericRepository;

            _logger = logger;
        }
        public Task<IEnumerable<VehicleLeave>> GetPlayerLeftVechile()
        {
            throw new NotImplementedException();
        }

        public async void InsertVehicleLeaveTelemetry(string jsonResult)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var lastestKillEventTimeStamp = jsonToJObject.Where(x => x.Value<string>("_T") == "LogVehicleLeave").Select(s => new { EventTimeStamp = s.Value<string>("_D") }).Last();

            var logvehicleLeave = GetLogVehicleLeave(jsonToJObject);

            var vehicleleaveTimeStamp = logvehicleLeave.Last().EventTimeStamp.ToDateTimeFormat();

            if (vehicleleaveTimeStamp > LastVehicleLeaveTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logvehicleLeave, "VehicleLeave");

                await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(logvehicleLeave, "VehicleLeave");

                LastVehicleLeaveTimeStamp = vehicleleaveTimeStamp;
            }
        }

        private IEnumerable<VehicleLeave> GetLogVehicleLeave(JArray jsonToJObject)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogVehicleLeave").Select(s => new VehicleLeave()
            {
                Character = new Character() {
                    Name = (string)s["character"]["name"],
                    TeamId = (int)s["character"]["teamId"],
                    Health = (float)s["character"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["character"]["location"]["x"],
                        y = (float)s["character"]["location"]["y"],
                        z = (float)s["character"]["location"]["z"],
                    },
                    Ranking = (int)s["character"]["ranking"],
                    AccountId = (string)s["character"]["accountId"]

                },
                Vehicle = new Vehicle() {
                    VehicleType = (string)s["vehicle"]["vehicleType"],
                    VehicleId = (string)s["vehicle"]["vehicleId"],
                    HealthPercent = (float)s["vehicle"]["healthPercent"],
                    FeulPercent = (float)s["vehicle"]["feulPercent"]
                },
                RideDistance = (float)s["rideDistance"],
                seatIndex = (int)s["seatIndex"],
                Common = new Common()
                {
                    MatchId = (string)s["common"]["matchId"],
                    MapName = (string)s["common"]["mapName"],
                    IsGame = (float)s["common"]["isGame"]

                },
                Version = (int)s["_V"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }
    }
}
