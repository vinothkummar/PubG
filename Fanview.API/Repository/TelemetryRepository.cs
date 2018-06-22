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

namespace Fanview.API.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private IHttpClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;        
        private IGenericRepository<Kill> _genericRepository;
        private IPlayerKillRepository _playerKillRepository;
        private ITakeDamageRepository _takeDamageRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        

        public TelemetryRepository(IHttpClientBuilder httpClientBuilder, IHttpClientRequest httpClientRequest, IGenericRepository<Kill> genericRepository, 
                                   IPlayerKillRepository playerKillRepository, ITakeDamageRepository takeDamageRepository,
                                   ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;          
            _genericRepository = genericRepository;
            _playerKillRepository = playerKillRepository;
            _takeDamageRepository = takeDamageRepository;
            _logger = logger;
        }
      

        public async void PollTelemetry()
        {
            var query = "pc-na/2018/06/07/00/59/0e690669-69ee-11e8-9d58-0a5864650332-telemetry.json";
            //var query = "pc-na/2018/06/11/18/20/11d976d4-6da4-11e8-ba97-0a586464d535-telemetry.json";
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

                    await Task.Run(async () => _takeDamageRepository.InsertTakeDamageTelemetry(jsonResult));

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
