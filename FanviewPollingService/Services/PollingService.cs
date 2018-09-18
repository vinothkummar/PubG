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
        }


       

        public void Start()
        {
            StartBase(); 

            Timers.Start("Poller", 700, () =>
            {
                _logger.LogInformation( "Service Started Polling " + Environment.NewLine );

                try
                {
                    _telemetryRepository.ReadUDPStreamFromFile();
                }
                catch (Exception excep)
                {
                    _logger.LogInformation("Service Polling Failure " + excep + Environment.NewLine);

                    throw;
                }

                ;

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
