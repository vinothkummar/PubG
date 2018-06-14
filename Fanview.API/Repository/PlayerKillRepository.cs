using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private IHttpClientRequest _httpClientRequest;
        private IHttpClientBuilder _httpClientBuilder;
        private IGenericRepository<PlayerKill> _genericRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime killEventlastTimeStamp = DateTime.MinValue;

        public PlayerKillRepository(IHttpClientBuilder httpClientBuilder, IHttpClientRequest httpClientRequest, IGenericRepository<PlayerKill> genericRepository, ILogger<PlayerKillRepository> logger)
        {
            _httpClientRequest = httpClientRequest;

            _httpClientBuilder = httpClientBuilder;

            _genericRepository = genericRepository;

            _logger = logger;

        }
        

        private static IEnumerable<PlayerKill> GetLogPlayerKillInfo(JArray jsonToJObject)
        {
            var result =  jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerKill").Select(s => new PlayerKill()
            {
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

        public async void GetPlayerKillTelemetry()
        {
            //var query = "pc-eu/2018/05/27/23/59/0edf9d73-620a-11e8-b75f-0a5864637c0e-telemetry.json";
            var query = "pc-na/2018/06/07/00/59/0e690669-69ee-11e8-9d58-0a5864650332-telemetry.json";
            try
            {
               _logger.LogInformation("Player Kill Telemetery Request Started" + Environment.NewLine);

               //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse =  _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), query);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Loading Player Kill Telemetery Response Json" + Environment.NewLine);

                     var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                     await Task.Run(async() => InsertPlayerKillTelemetry(jsonResult));

                     //InsertPlayerKillTelemetry(jsonResult);

                    _logger.LogInformation("Completed Loading Kill Telemetery Response Json" + Environment.NewLine);
                }
              
                _logger.LogInformation("Player Kill Telemetery Request Completed"  + Environment.NewLine);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async void InsertPlayerKillTelemetry(string jsonResult)
        {
           
            var jsonToJObject = JArray.Parse(jsonResult);

            var lastestKillEventTimeStamp = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerKill").Select(s => new {EventTimeStamp = s.Value<string>("_D") }).Last();

            IEnumerable<PlayerKill> logPlayerKill = GetLogPlayerKillInfo(jsonToJObject);

            var killEventTimeStamp = logPlayerKill.Last().EventTimeStamp.ToDateTimeFormat();            

            if (killEventTimeStamp > killEventlastTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logPlayerKill, "killMessages");

                await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(logPlayerKill, "killMessages");

                killEventlastTimeStamp = killEventTimeStamp;
            }
        }
    }
}
