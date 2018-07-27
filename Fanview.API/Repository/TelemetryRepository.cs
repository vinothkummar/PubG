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
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Fanview.API.Repository
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;        
        private IGenericRepository<Kill> _genericRepository;
        private IPlayerKillRepository _playerKillRepository;
        private ITakeDamageRepository _takeDamageRepository;
        private IPlayerRepository _playerVehicleLeaveRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;

        // Used for random suffixes on processed files to avoid
        // files with duplicate names in the same second.
        private Random _random = new Random();
        

        public TelemetryRepository(IClientBuilder httpClientBuilder, IHttpClientRequest httpClientRequest, IGenericRepository<Kill> genericRepository, 
                                   IPlayerKillRepository playerKillRepository, ITakeDamageRepository takeDamageRepository, IPlayerRepository playerVehicleLeaveRepository,
                                   ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;          
            _genericRepository = genericRepository;
            _playerKillRepository = playerKillRepository;
            _takeDamageRepository = takeDamageRepository;
            _playerVehicleLeaveRepository = playerVehicleLeaveRepository;
            _logger = logger;
        }
      

        public async void PollTelemetry()
        {
            var query = "2018/06/26/13/13/c845762d-7942-11e8-af88-0a5864603b03-telemetry.json";
            try
            {
                _logger.LogInformation("Telemetery Request Started" + Environment.NewLine);

                _pubGClientResponse = Task.Run(async () => await _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), query));

                //_pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), query);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Reading Telemetery Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () =>  _playerKillRepository.InsertPlayerKillTelemetry(jsonResult, string.Empty));

                    await Task.Run(async () => _playerVehicleLeaveRepository.InsertVehicleLeaveTelemetry(jsonResult, string.Empty));

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

        public void ReadUDPStreamFromFile()
        {
            _logger.LogInformation("UDP Streaming Read Started " + Environment.NewLine);
            try
            {
                var folderPathToReadFromFile = @"D:\LiveAPI2\test_live_multiple";

                var folderPathToMoveProcessedFile = @"D:\Processed";

                _logger.LogInformation("Telemetery Request Started" + Environment.NewLine);

                _logger.LogInformation("Telemetery folder path" + folderPathToReadFromFile + Environment.NewLine);

                _logger.LogInformation("Telemetery folder path" + folderPathToMoveProcessedFile + Environment.NewLine);


                foreach (var file in Directory.EnumerateFiles(folderPathToReadFromFile, "*.log"))
                {
                    _logger.LogInformation("Reading Streaming Data from file"+ file + Environment.NewLine);

                    string contents = File.ReadAllText(file);

                    var sections = file.Split('\\');

                    var fileName = sections[sections.Length - 1];

                    var objects = Deserializeobjects(contents);

                    var array = objects.ToArray();

                    _playerKillRepository.InsertLiveKillEventTelemetry(array, fileName);

                    _takeDamageRepository.InsertEventDamageTelemetry(array, fileName);

                    File.Move(file, folderPathToMoveProcessedFile + "\\" + fileName + "_" + _random.Next().ToString());
                }
                    
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private IEnumerable<JObject> Deserializeobjects(string jsonInput)
        {
            var serializer = new JsonSerializer();
            using(var strReader = new StringReader(jsonInput)){
                using(var jsonReader = new JsonTextReader(strReader))
                {
                    jsonReader.SupportMultipleContent = true;
                    while (jsonReader.Read())
                    {
                        yield return (JObject)serializer.Deserialize(jsonReader);
                    }
                }
            }
        }    
    }
}
