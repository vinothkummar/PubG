using Fanview.API.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;

namespace FanviewPollingService.Services
{
    public class PollingService : MicroService, IMicroService
    {
        private IMicroServiceController controller;        
       
        private IPlayerKillRepository _telemetryRepository;

        private ILogger<PollingService> _logger;

        public PollingService()
        {
            controller = null;
        }

        public PollingService(IMicroServiceController controller)
        {           
            this.controller = controller;

            var servicesProvider = ServiceConfiguration.BuildDI();  

           _telemetryRepository = servicesProvider.GetService<IPlayerKillRepository>();

            _logger = servicesProvider.GetService<ILogger<PollingService>>();
        }


       

        public void Start()
        {
            StartBase(); 

            Timers.Start("Poller", 1000, () =>
            {
                _logger.LogInformation( "Service Started Polling " + Environment.NewLine );

                _telemetryRepository.GetPlayerKillTelemetry();

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
