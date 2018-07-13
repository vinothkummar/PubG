using Fanview.API.GraphicsDummyData;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class PlayerKillRepository : IPlayerKillRepository
    {
        private IGenericRepository<Kill> _genericRepository;
        private LiveGraphichsDummyData _data;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime killEventlastTimeStamp = DateTime.MinValue;
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;

        public PlayerKillRepository(IClientBuilder httpClientBuilder,
                                    IHttpClientRequest httpClientRequest,
                                    IGenericRepository<Kill> genericRepository,
                                    ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;  
            _genericRepository = genericRepository;
            _data = new LiveGraphichsDummyData();

            _logger = logger;

        }
        

        private IEnumerable<Kill> GetLogPlayerKilled(JArray jsonToJObject, string matchId)
        {
            var result =  jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerKill").Select(s => new Kill()
            {
                MatchId = matchId,
                AttackId = s.Value<int>("attackId"),
                Killer = new Killer()
                {
                    Name = (string)s["killer"]["name"],
                    TeamId = (int)s["killer"]["teamId"],
                    Health = (float)s["killer"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["killer"]["location"]["x"],
                        y = (float)s["killer"]["location"]["y"],
                        z = (float)s["killer"]["location"]["z"],
                    },
                    Ranking = (int)s["killer"]["ranking"],
                    AccountId = (string)s["killer"]["accountId"]
                },
                Victim = new Victim()
                {
                    Name = (string)s["victim"]["name"],
                    TeamId = (int)s["victim"]["teamId"],
                    Health = (float)s["victim"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["victim"]["location"]["x"],
                        y = (float)s["victim"]["location"]["y"],
                        z = (float)s["victim"]["location"]["z"],
                    },
                    Ranking = (int)s["victim"]["ranking"],
                    AccountId = (string)s["victim"]["accountId"]
                },
                DamageTypeCategory = (string)s["damageTypeCategory"],               
                DamageCauserName = (string)s["damageCauserName"],
                Distance = (float)s["distance"],
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

        public async void InsertPlayerKillTelemetry(string jsonResult, string matchId)
        {              
            var jsonToJObject = JArray.Parse(jsonResult);

            var lastestKillEventTimeStamp = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerKill").Select(s => new {EventTimeStamp = s.Value<string>("_D") }).Last();

            IEnumerable<Kill> logPlayerKill = GetLogPlayerKilled(jsonToJObject, matchId);

            var killEventTimeStamp = logPlayerKill.Last().EventTimeStamp.ToDateTimeFormat();            

            if (killEventTimeStamp > killEventlastTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logPlayerKill, "Kill");

                await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(logPlayerKill, "killMessages");

                killEventlastTimeStamp = killEventTimeStamp;
            }
        }

        public async Task<IEnumerable<Kill>> GetPlayerKilled(string matchId)
        {
            _logger.LogInformation("GetPlayedKilled Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericRepository.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayedKilled Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayedKilled");

                throw;
            }            
        }



        public async Task<IEnumerable<Kill>> GetLast4PlayerKilled(string matchId)
        { 
            var response = _genericRepository.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId).TakeLast(4);

            return await Task.FromResult(response);
        }

        public async void PollTelemetryPlayerKilled(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Request Started" + Environment.NewLine);

                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Match Player Stats  Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    var _telemetryResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(GetTelemetryUrl(jsonResult)), string.Empty);

                    var telemetryJsonResult = _telemetryResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () => InsertPlayerKillTelemetry(telemetryJsonResult, matchId));

                    //await Task.Run(async () => InsertMatchPlayerStats(jsonResult));

                    //await Task.Run(async () => _takeDamageRepository.InsertTakeDamageTelemetry(jsonResult));

                    //InsertMatchSummary(jsonResult);

                    _logger.LogInformation("Completed Loading Match Player Stats  Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Match Player Stats  Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GetTelemetryUrl(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var match = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var telemetryAssets = playerResultsIncluded.Where(x => x.Value<string>("type") == "asset");

            var telemetryUrl = (string)telemetryAssets.Select(s => s.SelectToken("attributes.URL")).ElementAtOrDefault(0);

            return telemetryUrl;
        }

        public async Task<IEnumerable<Kill>> GetPlayerKilled(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            _logger.LogInformation("GetPlayedKilled Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericRepository.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId1 || cn.MatchId == matchId2 || cn.MatchId == matchId3 || cn.MatchId == matchId4);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayedKilled Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayedKilled");

                throw;
            }
        }

        public Task<KillLeaderList> GetKillLeaderList(string matchId)
        {
          return  Task.FromResult(_data.GetKillLeaderlist());
        }

        public Task<KillZone> GetKillZone(string matchId)
        {
            return Task.FromResult(_data.GetLiveKillzone());
        }
    }
}
