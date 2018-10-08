﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using MongoDB.Driver;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository
{
    public class MatchManagementRepository : IMatchManagementRepository
    {
        private IGenericRepository<Event> _tournamentRepository;
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private ILogger<MatchManagementRepository> _logger;

        public MatchManagementRepository(IGenericRepository<Event> tournamentRepository, IClientBuilder httpClientBuilder,
                                    IHttpClientRequest httpClientRequest, ILogger<MatchManagementRepository> logger)
        {
            _tournamentRepository = tournamentRepository;
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;
            _logger = logger;
        }

        public async Task<IEnumerable<Event>> GetMatchDetails()
        {
            var matchCollection = _tournamentRepository.GetMongoDbCollection("TournamentMatchId");

            return await matchCollection.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync();
        }

        public async Task<Object> GetTournaments()
        {
            try
            {
                _logger.LogInformation("Tournament Open API Request Started" + Environment.NewLine);

                var tournamentsResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "tournaments");

                if (tournamentsResponse.Result.StatusCode == HttpStatusCode.OK && tournamentsResponse != null)
                {
                    _logger.LogInformation("Tournament Open API Response Json" + Environment.NewLine);

                    var response = tournamentsResponse.Result.Content.ReadAsStringAsync().Result;                   

                    var o = JObject.Parse(response).SelectToken("data.id").ToList();

                   
                    
                   
                   
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void PostMatchDetails(Event matchDetails)
        {
            _tournamentRepository.Insert(matchDetails, "TournamentMatchId");
           
        }
    }
}
