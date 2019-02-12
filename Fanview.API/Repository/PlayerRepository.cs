using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
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
        private IMatchRepository _matchRepository;
        private IGenericRepository<VehicleLeave> _genericRepository;
        private IGenericRepository<ParachuteLanding> _parachuteLandingRepository;
        private ILogger<PlayerRepository> _logger;        
        private IGenericRepository<PlayerPoition> _PlayerPositionRepository;
        private ITeamPlayerRepository _teamPlayers;
        private DateTime LastVehicleLeaveTimeStamp = DateTime.MinValue;
      
        private IGenericRepository<Event> _tournament;

        public PlayerRepository(IMatchRepository matchRepository, IGenericRepository<VehicleLeave> genericRepository, 
             IGenericRepository<PlayerPoition> playerPositionRepository,
             ILogger<PlayerRepository> logger, IGenericRepository<Event> tournament, ITeamPlayerRepository teamPlayers,
             IGenericRepository<ParachuteLanding> parachuteLandingRepository
            )
        {
            _matchRepository = matchRepository;
            _genericRepository = genericRepository;
            _parachuteLandingRepository = parachuteLandingRepository;
            _PlayerPositionRepository = playerPositionRepository;
            _teamPlayers = teamPlayers;
            _logger = logger;
            _tournament = tournament;
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
                TeamId =  _teamPlayers.GetTeamPlayers().Result.Where( cn => cn.PlayerName == (string)s["character"]["name"]).FirstOrDefault().TeamIdShort,  //(int)s["character"]["teamId"],
                Health = (float)s["character"]["health"],
                Location = new Location()
                {
                    x = (float)s["character"]["location"]["x"],
                    y = (float)s["character"]["location"]["y"],
                    z = (float)s["character"]["location"]["z"],
                },
                AccountId = (string)s["character"]["accountId"],
                Ranking = (int)s["character"]["ranking"],
                elapsedTime = (int)s["elapsedTime"],
                NumAlivePlayers = (int)s["numAlivePlayers"],
                
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }
        public async void InsertVehicleLeaveTelemetry(string jsonResult, string matchId)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var vehicleLeave  = _genericRepository.GetMongoDbCollection("VehicleLeave");

            var isVehicleLeaveExists = await vehicleLeave.FindAsync(Builders<VehicleLeave>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isVehicleLeaveExists.Count() == 0 || isVehicleLeaveExists == null)
            {
                var logvehicleLeave = GetLogVehicleLeave(jsonToJObject, matchId);

                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logvehicleLeave, "VehicleLeave");

                await Task.Run(persistDataToMongo);
            }              
        }

        public async void InsertParachuteLanding(string jsonResult, string matchId)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var parachuteLanding = _parachuteLandingRepository.GetMongoDbCollection("ParachuteLanding");

            var isParachuteLandingExists = await parachuteLanding.FindAsync(Builders<ParachuteLanding>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isParachuteLandingExists.Count() == 0 || isParachuteLandingExists == null)
            {
                var logparachuteLanding = GetLogParachuteLanding(jsonToJObject, matchId);

                Func<Task> persistDataToMongo = async () => _parachuteLandingRepository.Insert(logparachuteLanding, "ParachuteLanding");

                await Task.Run(persistDataToMongo);
            }
        }

        private IEnumerable<ParachuteLanding> GetLogParachuteLanding(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogParachuteLanding").Select(s => new ParachuteLanding()
            {
                MatchId = matchId,
                Character = new Character()
                {
                    Name = (string)s["character"]["name"],
                    TeamId = _teamPlayers.GetTeamPlayers().Result.Where(cn => cn.PlayerName == (string)s["character"]["name"]).FirstOrDefault().TeamIdShort, //(int)s["character"]["teamId"],
                    Health = (float)s["character"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["character"]["location"]["x"],
                        y = (float)s["character"]["location"]["y"],
                        z = (float)s["character"]["location"]["z"],
                    },
                    Ranking = (int)s["character"]["ranking"],
                    AccountId = (string)s["character"]["isInBlueZone"],
                    IsInBlueZone = (bool)s["character"]["isInBlueZone"],
                    isInRedZone = (bool)s["character"]["isInRedZone"]
                },                
                Distance = (float)s["distance"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }

        private IEnumerable<VehicleLeave> GetLogVehicleLeave(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogVehicleLeave").Select(s => new VehicleLeave()
            {
                MatchId = matchId,
                Character = new Character() {
                    Name = (string)s["character"]["name"],
                    TeamId = _teamPlayers.GetTeamPlayers().Result.Where(cn => cn.PlayerName == (string)s["character"]["name"]).FirstOrDefault().TeamIdShort, //(int)s["character"]["teamId"],
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
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }

        public async Task<FlightPath> GetFlightPath(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var VehicleLeave = _genericRepository.GetMongoDbCollection("VehicleLeave");

            var vehicleTransport = VehicleLeave.FindAsync(Builders<VehicleLeave>.Filter.Where(cn => cn.MatchId == tournamentMatchId && cn.Vehicle.VehicleType =="TransportAircraft")).Result.ToListAsync().Result.OrderBy( o => o.Id);

            var flightPathfirstPosition = vehicleTransport.Take(1).FirstOrDefault();

            var flightPathlastPosition = vehicleTransport.TakeLast(1).LastOrDefault();

            var fpath = new FlightPath();

            fpath.MatchId = matchId;

            fpath.MapName = _matchRepository.GetMapName(tournamentMatchId).Result;

            fpath.FlightPathStart = flightPathfirstPosition.Character.Location;

            fpath.FlightPathEnd = flightPathlastPosition.Character.Location;


            return await Task.FromResult(fpath);
            
        }        
    }
} 
