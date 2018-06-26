using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net;
using System.Linq;
using Fanview.API.Utility;

namespace Fanview.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IAPIRequestBuilder _aPIRequestBuilder;
        private IGenericRepository<Event> _genericRepository;
        private ILogger<MatchRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime _lastMatchCreatedTimeStamp = DateTime.MinValue;

        public MatchRepository(IClientBuilder httpClientBuilder, IHttpClientRequest httpClientRequest,
                                      IAPIRequestBuilder aPIRequestBuilder,
                                      IGenericRepository<Event> genericRepository, ILogger<MatchRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;
            _aPIRequestBuilder = aPIRequestBuilder;           
            _genericRepository = genericRepository;          
            _logger = logger;
        }
        public async Task<JObject> GetMatchesDetailsByID(string id)
        {            
            var clientResponse = _httpClientRequest.GetAsync(await _aPIRequestBuilder.CreateRequestHeader(), "matches/"+id).Result;

            var jsonResult = clientResponse.Content.ReadAsStringAsync().Result;

            var jsonResultConvertedToJObjectObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return await Task.FromResult(jsonResultConvertedToJObjectObject);
        }

        public async void PollMatchSessionId(string eventName)
        {            
            try
            {
                _logger.LogInformation("Event Poll Request Started" + Environment.NewLine);

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), eventName);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Event Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    InsertEvent(jsonResult);

                    _logger.LogInformation("Completed Loading Telemetery Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Telemetery Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void InsertEvent(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

           // var currentMatchCreatedTimeStamp = jsonToJObject["included"].ToArray().Select(s => s["attributes"])
           //                                           .Select(s1 => s1["createdAt"]).OrderBy(o => o).Last()
           //                                           .ToString();

            
            
            var eventTournament =  GetTournament(jsonToJObject);



            //if (currentMatchCreatedTimeStamp.ToDateTimeFormat() > _lastMatchCreatedTimeStamp)
            //{
            //Func<Task> persistDataToMongo = async () => _genericRepository.Insert(matchSummaryData, "MatchPlayerStats");

            //await Task.Run(persistDataToMongo);

            _genericRepository.Insert(eventTournament, "Tournament");

            //    _lastMatchCreatedTimeStamp = currentMatchCreatedTimeStamp.ToDateTimeFormat();
            //}
        }

        private Event GetTournament(JObject jsonToJObject)
        {
            var result = jsonToJObject.SelectToken("data").ToObject<Event>();

            result.EventsDate = jsonToJObject["included"].Select(s => new EventsDate()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                EventsAttributes = new EventsAttributes() { CreatedAT = (string)s["attributes"]["createdAt"] }
            });
            return result;
        }

        public Task<JObject> GetMatchIdByTournament(string tournament)
        {
            throw new NotImplementedException();
        }
    }
}
