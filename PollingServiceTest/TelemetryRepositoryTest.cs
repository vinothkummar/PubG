using System;
using Xunit;
using FanviewPollingService.Services;
using FanviewPollingService.Contracts;
using FanviewPollingService.Repository.Interfaces;
using FanviewPollingService.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace PollingServiceTest
{
    public class TelemetryRepositoryTest
    {
        private ServiceProvider serviceProvider;
        private ITelemetryRepository _telemetryRepository;

        public TelemetryRepositoryTest()
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();

            _telemetryRepository  = serviceProvider.GetService<ITelemetryRepository>();

        }
        [Fact]
        public void Test1()
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            //for (int i = 0; i < 3; i++)
            //{
            //    _telemetryRepository.GetPlayerKillTelemetryJson();                
            //}

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);

        }
    }
}
