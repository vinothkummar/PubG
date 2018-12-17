using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<Event> _genericRepository;
        private IGenericRepository<RankPoints> _rankPointsRepository;
        private ILogger<MatchRepository> _logger;
        private IGenericRepository<MatchSafeZone> _matchSafeZoneRepository;
        private IGenericRepository<MatchSummaryData> _matchSummaryRepository;

        private ICacheService _cacheService;

        public MatchRepository(IClientBuilder httpClientBuilder, 
                               IHttpClientRequest httpClientRequest,                               
                               IGenericRepository<Event> genericRepository,
                               IGenericRepository<MatchSafeZone> matchSafeZoneRepository,
                               IGenericRepository<MatchSummaryData> matchSummaryRepository,
                               IGenericRepository<RankPoints> rankPointsRepository,
                               ICacheService cacheService,
                               ILogger<MatchRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;            
            _genericRepository = genericRepository;          
            _logger = logger;
            _matchSafeZoneRepository = matchSafeZoneRepository;
            _matchSummaryRepository = matchSummaryRepository;
            _rankPointsRepository = rankPointsRepository;
            _cacheService = cacheService;
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
                GameState = new GameState()
                {
                    ElapsedTime = (int)s["gameState"]["elapsedTime"],
                    NumAliveTeams = (int)s["gameState"]["numAliveTeams"],
                    NumJoinPlayers = (int)s["gameState"]["numJoinPlayers"],
                    NumStartPlayers = (int)s["gameState"]["numStartPlayers"],
                    NumAlivePlayers = (int)s["gameState"]["numAlivePlayers"],
                    SafetyZonePosition = new SafetyZonePosition(){
                        X = (float)s["gameState"]["safetyZonePosition"]["x"],
                        Y = (float)s["gameState"]["safetyZonePosition"]["y"],
                        Z = (float)s["gameState"]["safetyZonePosition"]["z"]
                    },
                    SafetyZoneRadius = (float)s["gameState"]["safetyZoneRadius"],
                    PoisonGasWarningPosition = new PoisonGasWarningPosition(){
                        X = (float)s["gameState"]["poisonGasWarningPosition"]["x"],
                        Y = (float)s["gameState"]["poisonGasWarningPosition"]["y"],
                        Z = (float)s["gameState"]["poisonGasWarningPosition"]["z"]
                    },
                    PoisonGasWarningRadius = (float)s["gameState"]["poisonGasWarningRadius"],
                    RedZonePosition = new RedZonePosition(){
                        X = (float)s["gameState"]["redZonePosition"]["x"],
                        Y = (float)s["gameState"]["redZonePosition"]["y"],
                        Z = (float)s["gameState"]["redZonePosition"]["z"]
                    },
                    RedZoneRadius = (float)s["gameState"]["redZoneRadius"]
                },
                //MatchSafeZoneCommon = new MatchSafeZoneCommon(){
                //    IsGame = (float)s["character"]["common"]["isGame"]
                //},
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }

        public async Task<Object> GetMatchSafeZone(int matchId)
        {
            var tournaments = _genericRepository.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var safeZone = _matchSafeZoneRepository.GetMongoDbCollection("MatchSafeZone");

            var matchSafeZone = safeZone.FindAsync(Builders<MatchSafeZone>.Filter.Where(cn => cn.MatchId == tournamentMatchId )).Result.ToListAsync().Result
                                            .Select(s => new SafeZone() { SafetyZonePosition = s.GameState.SafetyZonePosition,
                                                                                SafetyZoneRadius = s.GameState.SafetyZoneRadius }).GroupBy(g => new { x = g.SafetyZonePosition.X, y = g.SafetyZonePosition.Y, z = g.SafetyZonePosition.Z , Radius = g.SafetyZoneRadius });
             
            var findCircle = new List<SafeZone>();

            var findCircleDuplicate = new List<SafeZone>();
            var i = 1;
            foreach (var item in matchSafeZone)
            {
                if(item.Select(a => (a.SafetyZonePosition.X)).Count() > 1 && item.Select(a => (a.SafetyZonePosition.Y)).Count() > 1
                             && item.Select(a => (a.SafetyZonePosition.Z)).Count() > 1 && item.Select(a => a.SafetyZoneRadius).Count() > 1)
                {
                    findCircleDuplicate.Add(
                    new SafeZone()
                    {
                        circle = i++,
                        SafetyZonePosition = item.Select(a => a.SafetyZonePosition).ElementAtOrDefault(0),
                        SafetyZoneRadius = item.Select(a => a.SafetyZoneRadius).ElementAtOrDefault(0)
                    });

                }
            }

            Object matchSafeZonePositions = new 
            {
                MapName = GetMapName(tournamentMatchId).Result,
                safeZone = findCircleDuplicate

            };

            return await Task.FromResult(matchSafeZonePositions);
        }

        public async void InsertMatchSummary(string jsonResult, string matchId)
        {
            var matchSummary = _matchSummaryRepository.GetMongoDbCollection("MatchSummary");

            var isMatchSafeZoneExist = await matchSummary.FindAsync(Builders<MatchSummaryData>.Filter.Where(cn => cn.Id == matchId)).Result.ToListAsync();

            if (isMatchSafeZoneExist.Count() == 0 || isMatchSafeZoneExist == null)
            {
                var jsonToJObject = JObject.Parse(jsonResult);

                var matchSummaryData = jsonToJObject.SelectToken("data").ToObject<MatchSummaryData>();

                Func<Task> persistMatchSummaryToMongo = async () => _matchSummaryRepository.Insert(matchSummaryData, "MatchSummary");

                await Task.Run(persistMatchSummaryToMongo);
            }
        }

        public async Task<string> GetMapName(string matchId)
        {
            var matchSummary = _matchSummaryRepository.GetMongoDbCollection("MatchSummary");

            var mapName = await matchSummary.FindAsync(Builders<MatchSummaryData>.Filter.Where(cn => cn.Id == matchId)).Result.FirstOrDefaultAsync();

            return await Task.FromResult(mapName.Attributes.MapName);
        }

        public async Task<IEnumerable<RankPoints>> GetMatchRankPoints()
        {
            var cacheKey = "MatchRankPointsCache";

            var matchRankPointsCache =  _cacheService.RetrieveFromCache<IEnumerable<RankPoints>>(cacheKey);

            if (matchRankPointsCache != null)
            {
                return matchRankPointsCache;
            }
            else
            {
                var rankPoints = _rankPointsRepository.GetAll("RankPoints").Result.OrderByDescending(o => o.RankPosition);

                await _cacheService.SaveToCache<IEnumerable<RankPoints>>(cacheKey, rankPoints, 50, 10);

                return await Task.FromResult(rankPoints);
            }
        }
    }
}
