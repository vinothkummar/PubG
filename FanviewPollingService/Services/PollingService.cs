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
   
        static string Fileformatting
        {
            get
            {
                return string.Format("log-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            }
        }

        private string logFileName = Path.Combine(Path.GetFullPath(@"../../../../" + "PollingServiceLog"), Fileformatting);
        private ILogger<PollingService> _logger;

        public PollingService()
        {
            controller = null;
        }

        public PollingService(IMicroServiceController controller)
        {
            var servicesProvider = ServiceConfiguration.BuildDI();           
            
            _telemetryRepository = servicesProvider.GetService<ITelemetryRepository>();            

            this.controller = controller;

            Log.Logger = new LoggerConfiguration().WriteTo.File(logFileName).CreateLogger();

            var serviceCollection = new ServiceCollection();

            ServiceConfiguration.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = serviceProvider.GetService<ILogger<PollingService>>();
        }


       

        public async void Start()
        {
            StartBase(); 

            Timers.Start("Poller", 1000, () =>
            {
                
              _logger.LogInformation 

              _telemetryRepository.GetPlayerKillTelemetryJson();

            // _telemetryRepository.InsertPlayerKillTelemetry();

            
            });
        }

        public void Stop()
        {
            StopBase();

            File.AppendAllText(logFileName, "Stopped\n");
            Console.WriteLine("I stopped");
        }

    }
}
