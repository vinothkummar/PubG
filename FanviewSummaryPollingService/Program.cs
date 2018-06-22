using PeterKottas.DotNetCore.WindowsService;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FanviewEventPollingService.Services;
using Serilog;
using System.Collections.Generic;

namespace FanviewEventPollingService
{
    class Program
    {

        static string SpecialFileName
        {
            get
            {
                return string.Format("Match-Event-Poll-log-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            }
        }

        public static void Main(string[] args)
        {
            var fileName = Path.Combine(Path.GetFullPath(@"../../../../../" + "PollingServiceLog"), SpecialFileName);

            Log.Logger = new LoggerConfiguration().WriteTo.File(fileName).CreateLogger();


            var serviceProvider = ServiceConfiguration.BuildDI();

            var logger = serviceProvider.GetService<ILogger<Program>>();

           // var eventName = new List<string>();

            Console.WriteLine("Please Enter the Event Name: ");

            var eventName = Console.ReadLine();

            ServiceRunner<PollingService>.Run(config =>
            {
                var name = config.GetDefaultName();

                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        logger.LogInformation("Fanview Polling initializing" + Environment.NewLine);

                        return new PollingService(controller, eventName);
                    });

                    serviceConfig.OnStart((service, extraParams) =>
                    {
                        Console.WriteLine("Service {0} started", name);
                        service.Start();
                    });

                    serviceConfig.OnStop(service =>
                    {
                        Console.WriteLine("Service {0} stopped", name);
                        service.Stop();
                    });

                    serviceConfig.OnError(e =>
                    {
                        logger.LogError("Exception: {Exception}", e.ToString());
                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });
            });
        }
    }
}
