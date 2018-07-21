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
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private IGenericRepository<VehicleLeave> _genericRepository;
        private ILogger<PlayerRepository> _logger;        
        private IGenericRepository<PlayerPoition> _PlayerPositionRepository;
        private DateTime LastVehicleLeaveTimeStamp = DateTime.MinValue;
        private IGenericRepository<TeamPlayer> _teamPlayers;

        public PlayerRepository(IGenericRepository<VehicleLeave> genericRepository,
             IGenericRepository<PlayerPoition> playerPositionRepository,
             IGenericRepository<TeamPlayer> teamPlayers, ILogger<PlayerRepository> logger)
        {
            _genericRepository = genericRepository;
            _PlayerPositionRepository = playerPositionRepository;
            _teamPlayers = teamPlayers;
            _logger = logger;
        }
        public Task<IEnumerable<VehicleLeave>> GetPlayerLeftVechile()
        {
            throw new NotImplementedException();
        }

        public async void InsertLogPlayerPosition(string jsonResult, string matchId)
        {
            var jsonToJObject = JArray.Parse(jsonResult);            

            var playerCreated = _PlayerPositionRepository.GetMongoDbCollection("PlayerPosition");

            var isPlayerPositionExists = await playerCreated.FindAsync(Builders<PlayerPoition>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isPlayerPositionExists.Count() == 0 || isPlayerPositionExists == null){

                IEnumerable<PlayerPoition> logPlayerPosition = GetLogPlayerPosition(jsonToJObject, matchId);
          
                Func<Task> persistPlayerToMongo = async () => _PlayerPositionRepository.Insert(logPlayerPosition, "PlayerPosition");

                await Task.Run(persistPlayerToMongo);
            }
        }

        private IEnumerable<PlayerPoition> GetLogPlayerPosition(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerPosition").Select(s => new PlayerPoition()
            {
                MatchId = matchId,
                Name = (string)s["character"]["name"],
                TeamId = (int)s["character"]["teamId"],
                Health = (float)s["character"]["health"],
                Location = new Location()
                {
                    x = (float)s["character"]["location"]["x"],
                    y = (float)s["character"]["location"]["y"],
                    z = (float)s["character"]["location"]["z"],
                },
                AccountId = (string)s["character"]["accountId"],
                //Ranking = (int)s["character"]["accountId"],
                //elapsedTime = (string)s["elapsedTime"],
                NumAlivePlayers = (int)s["numAlivePlayers"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }

        public async void InsertVehicleLeaveTelemetry(string jsonResult, string matchId)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var lastestKillEventTimeStamp = jsonToJObject.Where(x => x.Value<string>("_T") == "LogVehicleLeave").Select(s => new { EventTimeStamp = s.Value<string>("_D") }).Last();

            var logvehicleLeave = GetLogVehicleLeave(jsonToJObject, matchId);

            var vehicleleaveTimeStamp = logvehicleLeave.Last().EventTimeStamp.ToDateTimeFormat();

            if (vehicleleaveTimeStamp > LastVehicleLeaveTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logvehicleLeave, "VehicleLeave");

                await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(logvehicleLeave, "VehicleLeave");

                LastVehicleLeaveTimeStamp = vehicleleaveTimeStamp;
            }
        }

        private IEnumerable<VehicleLeave> GetLogVehicleLeave(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogVehicleLeave").Select(s => new VehicleLeave()
            {
                MatchId = matchId,
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
