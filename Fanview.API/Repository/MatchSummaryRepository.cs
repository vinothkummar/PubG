using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using Fanview.API.Services.Interface;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Fanview.API.Utility;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class MatchSummaryRepository : IMatchSummaryRepository
    {
        private IClientBuilder _httpClientBuilder;        
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<MatchSummary> _genericMatchSummaryRepository;
        private IGenericRepository<MatchPlayerStats> _genericMatchPlayerStatsRepository;
        private IGenericRepository<TeamPlayer> _genericTeamPlayerRepository;
        private IGenericRepository<MatchRanking> _genericMatchRankingRepository;
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _teamPlayerRepository;
        private IPlayerKillRepository _playerKillRepository;       
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime LastMatchCreatedTimeStamp = DateTime.MinValue;

        public MatchSummaryRepository(IClientBuilder httpClientBuilder,
                                      IHttpClientRequest httpClientRequest,
                                      IGenericRepository<MatchSummary> genericMatchSummaryRepository,
                                      IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
                                      IGenericRepository<TeamPlayer> genericTeamPlayerRepository,
                                      IGenericRepository<MatchRanking> genericMatchRankingRepository,
                                      ITeamRepository teamRepository,
                                      ITeamPlayerRepository teamPlayerRepository,
                                      IPlayerKillRepository playerKillRepository,
                                      ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;            
            _httpClientRequest = httpClientRequest;
            _genericMatchSummaryRepository = genericMatchSummaryRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            _genericTeamPlayerRepository = genericTeamPlayerRepository;
            _genericMatchRankingRepository = genericMatchRankingRepository;
            _teamRepository = teamRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _playerKillRepository = playerKillRepository;
            _logger = logger;
        }

        private MatchSummary GetMatchSummaryData(JObject jsonToJObject)
        {
            var result = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var participantMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "participant");

            result.MatchParticipant = participantMatchPlayerStats.Select(s => new MatchParticipant()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                ParticipantAttributes = s.SelectToken("attributes").ToObject<ParticipantAttributes>()
            });

            var rosterMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "roster");

            result.MatchRoster = rosterMatchPlayerStats.Select(s => new MatchRoster()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                RosterAttributes = s.SelectToken("attributes").ToObject<RosterAttributes>(),
                RosterRelationShips = new RosterRelationShips()
                {
                    Participant = s.SelectToken("relationships.participants.data").Select(p =>  new Participant()
                    {
                        Id = (string)p["id"],
                        Type = (string)p["type"]
                    })
                }
            });
            
            return result;
        }

        public async void InsertMatchSummary(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var CurrentMatchCreatedTimeStamp = (string)jsonToJObject["data"]["attributes"]["createdAt"];

            var matchSummaryData = GetMatchSummaryData(jsonToJObject);           

            //var CurrentMatchCreatedTimeStamp = matchSummaryData.Attributes.CreatedAT.ToDateTimeFormat();

            if (CurrentMatchCreatedTimeStamp.ToDateTimeFormat()  >  LastMatchCreatedTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericMatchSummaryRepository.Insert(matchSummaryData, "MatchSummary");

                 await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(matchSummaryData, "MatchSummary");

                LastMatchCreatedTimeStamp = CurrentMatchCreatedTimeStamp.ToDateTimeFormat();
            }
        }


        public async void PollMatchSummary(string matchId)
        {   
            try
            {
                _logger.LogInformation("Poll Match Summary Request Started" + Environment.NewLine);
                 
                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Poll Match Summary Requested Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                     await Task.Run(async () => InsertMatchSummary(jsonResult));

                    //await Task.Run(async () => _takeDamageRepository.InsertTakeDamageTelemetry(jsonResult));

                    //InsertMatchSummary(jsonResult);

                    _logger.LogInformation("Completed Loading Poll Match Summary Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Poll Match Summary Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async void PollMatchParticipantStats(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Player Stats Request Started" + Environment.NewLine);

                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Match Player Stats  Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () => InsertMatchPlayerStats(jsonResult, matchId));

                    _logger.LogInformation("Completed Loading Match Player Stats  Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Match Player Stats  Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void InsertMatchPlayerStats(string jsonResult, string matchId)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var matchPlayerStatsCollection = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");

            var isMatchStatsExists = await matchPlayerStatsCollection.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isMatchStatsExists.Count()  == 0 || isMatchStatsExists == null )
            {
                var matchPlayerStats = GetMatchPlayerStas(jsonToJObject, matchId);

                Func<Task> persistDataToMongo = async () => _genericMatchPlayerStatsRepository.Insert(matchPlayerStats, "MatchPlayerStats");

                await Task.Run(persistDataToMongo);
            }
        }
        private IEnumerable<MatchPlayerStats> GetMatchPlayerStas(JObject jsonToJObject, string matchId)
        {
            var match = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var rosterMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "roster");

            var matchRoster = rosterMatchPlayerStats.Select(s => new MatchRoster()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                RosterAttributes = s.SelectToken("attributes").ToObject<RosterAttributes>(),
                RosterRelationShips = new RosterRelationShips()
                {
                    Participant = s.SelectToken("relationships.participants.data").Select(p => new Participant()
                    {
                        Id = (string)p["id"],
                        Type = (string)p["type"]
                    })
                }
            });
           
            var participantMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "participant");

            var matchParticipant = participantMatchPlayerStats.Select(s => new MatchParticipant()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                ParticipantAttributes = s.SelectToken("attributes").ToObject<ParticipantAttributes>()
            });

            var teamPlayers = _teamPlayerRepository.GetTeamPlayers(matchId);

            var teamParticipants = new List<MatchPlayerStats>();

            foreach (var item in matchRoster)
            {
                foreach (var item1 in item.RosterRelationShips.Participant )
                {
                    foreach (var item2 in matchParticipant)
                    {                        
                            var teamParticipant = new MatchPlayerStats();

                            if (item1.Id == item2.Id)
                            {
                                teamParticipant.MatchId = match.Id;
                                teamParticipant.RosterId = item.Id;
                                teamParticipant.ParticipantId = item2.Id;                              
                                teamParticipant.stats = item2.ParticipantAttributes.stats;
                                teamParticipant.TeamId = teamPlayers.Result.Where(cn => cn.PubgAccountId == item2.ParticipantAttributes.stats.PlayerId).FirstOrDefault().TeamId;
                                teamParticipants.Add(teamParticipant);
                        }
                    }
                }
            }
            return teamParticipants;
        }

        public void CreateAndMapTestTeamPlayerFromMatchHistory(string matchId)
        {
            var teamPlayers = new List<TeamPlayer>();

            var matchPlayerStatus = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;

            foreach (var item1 in matchPlayerStatus.Where(cn => cn.MatchId == matchId).GroupBy(g => g.TeamId))
            {
                foreach (var item2 in item1)
                {
                    var teamPlayer = new TeamPlayer();

                    teamPlayer.MatchId = item2.MatchId;
                    teamPlayer.PlayerName = item2.stats.Name;
                    teamPlayer.PubgAccountId = item2.stats.PlayerId;
                    teamPlayer.TeamId = item2.TeamId;

                    teamPlayers.Add(teamPlayer);
                }
            }

            _genericTeamPlayerRepository.Insert(teamPlayers, "TeamPlayers");

        }

        public async Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId)
        {
            _logger.LogInformation("GetPlayerMatchStats Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result.Where(cn => cn.MatchId == matchId);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayerMatchStats Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerMatchStats");

                throw;
            }
        }

        public async Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            _logger.LogInformation("GetPlayerMatchStats Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result.Where(cn => cn.MatchId == matchId1 || cn.MatchId == matchId2 || cn.MatchId == matchId3 || cn.MatchId == matchId4);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayerMatchStats Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerMatchStats");

                throw;
            }
        }

        public async Task PollMatchRoundRankingData(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Round Ranking Data Request Started" + Environment.NewLine);               

                await Task.Run(async () => PollMatchParticipantStats(matchId));

                await Task.Run(async () => _playerKillRepository.PollTelemetryPlayerKilled(matchId));

                _logger.LogInformation("Match Round Ranking Data Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
