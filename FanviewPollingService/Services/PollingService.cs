using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FanviewPollingService.Contracts;
using FanviewPollingService.Repository;
using FanviewPollingService.Repository.Interfaces;
using FanviewPollingService.Services;
using MongoDB.Bson;
using FanviewPollingService.Services;
using System.Diagnostics;
using Serilog;

namespace FanviewPollingService.Services
{
    public class PollingService : MicroService, IMicroService
    {
        private IMicroServiceController controller;        
        private IHttpClientRequest _servicerRequest;
        private IHttpClientBuilder _httpClient;
        private ITelemetryRepository _telemetryRepository;

        private ILogger<PollingService> _logger;

        public PollingService()
        {
            controller = null;
        }

        public PollingService(IMicroServiceController controller)
        {           
            this.controller = controller;

            var servicesProvider = ServiceConfiguration.BuildDI();

            _telemetryRepository = servicesProvider.GetService<ITelemetryRepository>();

            _logger = servicesProvider.GetService<ILogger<PollingService>>();

            _telemetryRepository = servicesProvider.GetService<ITelemetryRepository>();
        }


       

        public async void Start()
        {
            StartBase(); 

            Timers.Start("Poller", 1000, () =>
            {

                _logger.LogInformation( "Service Started Polling " + Environment.NewLine );

                _telemetryRepository.GetPlayerKillTelemetryJson();

                _logger.LogInformation( "Service Completed Polling "+ Environment.NewLine );
            });
        }

        public void Stop()
        {
            StopBase();

             _logger.LogInformation(Environment.NewLine + "Service Stopped @ {0:yyyy-MM-dd_hh-mm-ss-tt-fff}", DateTime.Now.ToString("o") + Environment.NewLine);

            Console.WriteLine("I stopped");
        }

    }
}
