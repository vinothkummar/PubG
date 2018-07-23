﻿using Fanview.API.GraphicsDummyData;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
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
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<Event> _genericRepository;
        private ILogger<MatchRepository> _logger;
        private LiveGraphichsDummyData _data;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private IGenericRepository<MatchSafeZone> _matchSafeZoneRepository;

        public MatchRepository(IClientBuilder httpClientBuilder, 
                               IHttpClientRequest httpClientRequest,                               
                               IGenericRepository<Event> genericRepository,
                               IGenericRepository<MatchSafeZone> matchSafeZoneRepository,
                               ILogger<MatchRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;            
            _genericRepository = genericRepository;          
            _logger = logger;
            _data = new LiveGraphichsDummyData();
            _matchSafeZoneRepository = matchSafeZoneRepository;
        }
        public async Task<JObject> GetMatchesDetailsByID(string id)
        {            
            var clientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + id).Result;

            var jsonResult = clientResponse.Content.ReadAsStringAsync().Result;

            var jsonResultConvertedToJObjectObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return await Task.FromResult(jsonResultConvertedToJObjectObject);
        }

        //public async void PollMatchSessionId(string eventName)
        //{            
        //    try
        //    {
        //        _logger.LogInformation("Event Poll Request Started" + Environment.NewLine);

        //        _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), eventName);

        //        if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
        //        {
        //            _logger.LogInformation("Reading Event Response Json" + Environment.NewLine);

        //            var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

        //            InsertEvent(jsonResult, eventName);

        //            _logger.LogInformation("Completed Loading Event Response Json" + Environment.NewLine);
        //        }

        //        _logger.LogInformation("Event Poll Request Completed" + Environment.NewLine);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation("Event Poll Request Completed {Exception}" + Environment.NewLine, ex);
        //        throw;
        //    }

        //}

        //public async void InsertEvent(string jsonResult, string eventName)
        //{
        //    var jsonToJObject = JObject.Parse(jsonResult);

        //    var eventTournament =  CreateEventObject(jsonToJObject, eventName);


        //     var tournamentMatchIds = GetTournamentMatchId();
             
        //     if (tournamentMatchIds.Result.Where(cn => eventTournament.Select(s => s.Id).Contains(cn.Id)).Count() == 0){

        //        Func<Task> persistDataToMongo = async () => _genericRepository.Insert(eventTournament, "TournamentMatchId");

        //        await Task.Run(persistDataToMongo);
        //       // _genericRepository.Insert(eventTournament, "Tournament");
        //    }
        //}

        //private IEnumerable<Event> CreateEventObject(JObject jsonToJObject, string eventName)
        //{  
        //    var eventRoundId = jsonToJObject.SelectToken("data.relationships.matches.data").Select(s => new Matches() {Id = (string)s["id"]});

        //    var eventRoundCreated = jsonToJObject["included"].Select(s => new EventsDate()
        //    {
        //        Id = (string)s["id"],
        //        Type = (string)s["type"],               
        //        CreatedAT = (string)s["attributes"]["createdAt"]
        //    });

           
        //    var tMatches = eventRoundCreated.Join(eventRoundId, erc => erc.Id, er => er.Id, (erc, er) => new { erc, er })
        //                      .OrderBy(o => o.erc.CreatedAT)
        //                      .Select(s => new Event()
        //                      {
        //                          Id = s.erc.Id,
        //                          Type = s.erc.Type,
        //                          CreatedAT = s.erc.CreatedAT,
        //                          EventName = eventName,                                
        //                      });

        //    var eventMatches = new List<Event>();

        //    var i = 1;

        //    foreach (var item in tMatches)
        //    {
        //        var match = new Event()
        //                    {
        //                        Id = item.Id,
        //                        Type = item.Type,
        //                        CreatedAT = item.CreatedAT,
        //                        EventName = eventName,
        //                        MatchId = i++
        //                    };

        //        eventMatches.Add(match);

               
        //    }


        //    return eventMatches;
        //}

       

        public async Task<IEnumerable<Event>> GetTournamentMatchId()
        {

            var response = _genericRepository.GetAll("TournamentMatchId");

            return await response;
        }
        public Task<JObject> GetMatchIdByTournament(string tournament)
        {
            throw new NotImplementedException();
        }

        public Task<FlightPath> GetFlightPath()
        {
            return Task.FromResult(_data.GetFlightPath());
        }

        public async void InsertMatchSafeZonePosition(string jsonResult, string matchId)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var matchSafeZone = _matchSafeZoneRepository.GetMongoDbCollection("MatchSafeZone");

            var isMatchSafeZoneExist = await matchSafeZone.FindAsync(Builders<MatchSafeZone>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isMatchSafeZoneExist.Count() == 0 || isMatchSafeZoneExist == null)
            {
                IEnumerable<MatchSafeZone> matchSafeZonePosition = GetMatchSafeZonePosition(jsonToJObject, matchId);

                Func<Task> persistMatchSafeZoneToMongo = async () => _matchSafeZoneRepository.Insert(matchSafeZonePosition, "MatchSafeZone");

                await Task.Run(persistMatchSafeZoneToMongo);
            }
        } 

        private IEnumerable<MatchSafeZone> GetMatchSafeZonePosition(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogGameStatePeriodic").Select(s => new MatchSafeZone()
            {
                MatchId = matchId,
                GameState = new GameState(){
                    ElapsedTime = (int)s["character"]["elapsedTime"],
                    NumAliveTeams = (int)s["character"]["numAliveTeams"],
                    NumJoinPlayers = (int)s["character"]["numJoinPlayers"],
                    NumStartPlayers = (int)s["character"]["numStartPlayers"],
                    NumAlivePlayers = (int)s["character"]["numAlivePlayers"],
                    SafetyZonePosition = new SafetyZonePosition(){
                        X = (float)s["character"]["safetyZonePosition"]["x"],
                        Y = (float)s["character"]["safetyZonePosition"]["y"],
                        Z = (float)s["character"]["safetyZonePosition"]["z"]
                    },
                    SafetyZoneRadius = (float)s["character"]["safetyZoneRadius"],
                    PoisonGasWarningPosition = new PoisonGasWarningPosition(){
                        X = (float)s["character"]["poisonGasWarningPosition"]["x"],
                        Y = (float)s["character"]["poisonGasWarningPosition"]["y"],
                        Z = (float)s["character"]["poisonGasWarningPosition"]["z"]
                    },
                    PoisonGasWarningRadius = (float)s["character"]["poisonGasWarningRadius"],
                    RedZonePosition = new RedZonePosition(){
                        X = (float)s["character"]["redZonePosition"]["x"],
                        Y = (float)s["character"]["redZonePosition"]["y"],
                        Z = (float)s["character"]["redZonePosition"]["z"]
                    },
                    RedZoneRadius = (float)s["character"]["redZoneRadius"]
                },
                MatchSafeZoneCommon = new MatchSafeZoneCommon(){
                    IsGame = (float)s["character"]["common"]["isGame"]
                },
                _D = (string)s["_D"],
                _T = (string)s["_T"]
            });

            return result;
        }


    }
}
