﻿using Fanview.API.Model;
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

namespace Fanview.API.Repository
{
    public class MatchSummaryRepository : IMatchSummaryRepository
    {
        private IClientBuilder _httpClientBuilder;        
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<MatchSummary> _genericMatchSummaryRepository;
        private IGenericRepository<MatchPlayerStats> _genericMatchPlayerStatsRepository;
        private ITeamRepository _teamRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime LastMatchCreatedTimeStamp = DateTime.MinValue;
        
        public MatchSummaryRepository(IClientBuilder httpClientBuilder,
                                      IHttpClientRequest httpClientRequest,                                      
                                      IGenericRepository<MatchSummary> genericMatchSummaryRepository,
                                      IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
                                      ITeamRepository teamRepository,
                                      ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;            
            _httpClientRequest = httpClientRequest;
            _genericMatchSummaryRepository = genericMatchSummaryRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            _teamRepository = teamRepository;
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

                    await Task.Run(async () => InsertMatchPlayerStats(jsonResult));

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

        private async void InsertMatchPlayerStats(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var CurrentMatchCreatedTimeStamp = (string)jsonToJObject["data"]["attributes"]["createdAt"];

            var matchPlayerStats = GetMatchPlayerStas(jsonToJObject);

            //var dummyTeamPlayers = CreateDummyTeamPlayers(matchPlayerStats);

            //var CurrentMatchCreatedTimeStamp = matchSummaryData.Attributes.CreatedAT.ToDateTimeFormat();

            //if (CurrentMatchCreatedTimeStamp.ToDateTimeFormat() > LastMatchCreatedTimeStamp)
            //{
                    Func<Task> persistDataToMongo = async () => _genericMatchPlayerStatsRepository.Insert(matchPlayerStats, "MatchPlayerStats");

                    await Task.Run(persistDataToMongo);

            //_genericRepository.Insert(matchSummaryData, "MatchSummary");

            //    LastMatchCreatedTimeStamp = CurrentMatchCreatedTimeStamp.ToDateTimeFormat();
            //}
        }

        private TeamPlayer CreateDummyTeamPlayers(IEnumerable<MatchPlayerStats> matchPlayers)
        {

            var teamPlayers = new List<TeamPlayer>();

            var team = _teamRepository.GetTeam().Result;

            var teamParticipants = matchPlayers.GroupBy(g => g.RosterId);
           
                foreach (var item1 in teamParticipants)
                {
                    foreach (var item2 in item1)
                    {
                        var teamPlayer = new TeamPlayer();

                        teamPlayer.MatchId = item2.MatchId;                       
                        teamPlayer.PlayerName = item2.stats.Name;
                        teamPlayer.PubgAccountId = item2.stats.PlayerId;

                        teamPlayers.Add(teamPlayer);
                    }
                }
            
           

            

            throw new NotImplementedException();
        }

        private IEnumerable<MatchPlayerStats> GetMatchPlayerStas(JObject jsonToJObject)
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
                                teamParticipants.Add(teamParticipant);
                        }
                    }
                }
            }
            return teamParticipants;
        }

    }
}