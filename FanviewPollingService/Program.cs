using PeterKottas.DotNetCore.WindowsService;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FanviewPollingService.Services;
using Serilog;

namespace FanviewPollingService
{
    class Program
    {
        
        static string SpecialFileName
        {
            get
            {                
                return string.Format("log-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            }
        }
                
        public static void Main(string[] args)
        {
            var fileName = Path.Combine(Path.GetFullPath(@"../../../../../" + "PollingServiceLog"), SpecialFileName);

            Log.Logger = new LoggerConfiguration().WriteTo.File(fileName).CreateLogger();

            var serviceCollection = new ServiceCollection();

            ServiceConfiguration.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();

            ServiceRunner<PollingService>.Run(config =>
            {
                var name = config.GetDefaultName();

                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        logger.LogInformation("Fanview Polling initializing");

                        return new PollingService(controller);
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
                        //File.AppendAllText(fileName, $"Exception: {e.ToString()}\n");
                        logger.LogError("Exception: {Exception}", e.ToString());
                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });
            });
        }
    }
}
