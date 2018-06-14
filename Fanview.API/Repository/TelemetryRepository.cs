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

namespace Fanview.API.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private IHttpClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;        
        private IGenericRepository<PlayerKill> _genericRepository;
        private IPlayerKillRepository _playerKillRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        

        public TelemetryRepository(IHttpClientBuilder httpClientBuilder, IHttpClientRequest httpClientRequest, IGenericRepository<PlayerKill> genericRepository, IPlayerKillRepository playerKillRepository, ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;          
            _genericRepository = genericRepository;
            _playerKillRepository = playerKillRepository;
            _logger = logger;
        }
        public Task<IEnumerable<PlayerKill>> GetPlayerKills()
        {
            
            var result = _genericRepository.GetAll("killMessages");

            return result;
        }

        public async void GetTelemetry()
        {
            //var query = "pc-eu/2018/05/27/23/59/0edf9d73-620a-11e8-b75f-0a5864637c0e-telemetry.json";
            var query = "pc-na/2018/06/07/00/59/0e690669-69ee-11e8-9d58-0a5864650332-telemetry.json";
            try
            {
                _logger.LogInformation("Telemetery Request Started" + Environment.NewLine);

                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), query);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Reading Telemetery Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () => _playerKillRepository.InsertPlayerKillTelemetry(jsonResult));

                    //InsertPlayerKillTelemetry(jsonResult);

                    _logger.LogInformation("Completed Loading Telemetery Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Telemetery Request Completed" + Environment.NewLine);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}