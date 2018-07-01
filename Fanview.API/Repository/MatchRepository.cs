﻿using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<Event> _genericRepository;
        private ILogger<MatchRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;

        public MatchRepository(IClientBuilder httpClientBuilder, 
                               IHttpClientRequest httpClientRequest,                               
                               IGenericRepository<Event> genericRepository,
                               ILogger<MatchRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;            
            _genericRepository = genericRepository;          
            _logger = logger;
        }
        public async Task<JObject> GetMatchesDetailsByID(string id)
        {            
            var clientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + id).Result;

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
                    _logger.LogInformation("Reading Event Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    InsertEvent(jsonResult, eventName);

                    _logger.LogInformation("Completed Loading Event Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Event Poll Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Event Poll Request Completed {Exception}" + Environment.NewLine, ex);
                throw;
            }

        }

        public async void InsertEvent(string jsonResult, string eventName)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var eventTournament =  CreateEventObject(jsonToJObject, eventName);


             var tournamentMatchIds = GetTournamentMatchId();
             
             if (tournamentMatchIds.Result.Where(cn => eventTournament.Select(s => s.Id).Contains(cn.Id)).Count() == 0){

                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(eventTournament, "TournamentMatchId");

                await Task.Run(persistDataToMongo);
               // _genericRepository.Insert(eventTournament, "Tournament");
            }
        }

        private IEnumerable<Event> CreateEventObject(JObject jsonToJObject, string eventName)
        {  
            var eventRoundId = jsonToJObject.SelectToken("data.relationships.matches.data").Select(s => new Matches() {Id = (string)s["id"]});

            var eventRoundCreated = jsonToJObject["included"].Select(s => new EventsDate()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],               
                CreatedAT = (string)s["attributes"]["createdAt"]
            });

           var tournamentMatches = eventRoundCreated.Join(eventRoundId, erc => erc.Id, er => er.Id, (erc, er) => new { erc, er })
                             .Select(s => new Event() {
                                 Id = s.erc.Id,
                                 Type = s.erc.Type,
                                 CreatedAT = s.erc.CreatedAT,
                                 EventName = eventName
                             }).OrderBy(o => o.CreatedAT);

          
            return tournamentMatches;
        }

        public async Task<IEnumerable<Event>> GetTournamentMatchId()
        {

            var response = _genericRepository.GetAll("TournamentMatchId");

            return await response;
        }
        public Task<JObject> GetMatchIdByTournament(string tournament)
        {
            throw new NotImplementedException();
        }
    }
}
