using Fanview.API.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;

namespace FanviewEventPollingService.Services
{
    public class PollingService : MicroService, IMicroService
    {
        private IMicroServiceController controller;
        private string eventName;
        private IMatchRepository _matchRepository;
        private ILogger<PollingService> _logger;

        public PollingService()
        {
            controller = null;
        }

        public PollingService(IMicroServiceController controller, string eventName)
        {           
            this.controller = controller;

            this.eventName = eventName;

            var servicesProvider = ServiceConfiguration.BuildDI();
            
           _matchRepository  = servicesProvider.GetService<IMatchRepository>();

            _logger = servicesProvider.GetService<ILogger<PollingService>>();
        }


       

        public void Start()
        {
            StartBase(); 

            Timers.Start("Poller", 10000, () =>
            {
                _logger.LogInformation( "Service Started Polling " + Environment.NewLine );

                //_matchRepository.PollMatchSessionId(eventName);

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
