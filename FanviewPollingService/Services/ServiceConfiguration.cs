using FanviewPollingService.Contracts;
using FanviewPollingService.Repository;
using FanviewPollingService.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace FanviewPollingService.Services
{
    public static class ServiceConfiguration
    {
        public static IServiceProvider BuildDI()
        {
            var serviceProvider = new ServiceCollection().AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace))
                                                         .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                                                         .AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
                                                         .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                                                         .AddTransient<ITelemetryRepository, TelemetryRepository>()
                                                         .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //var loadLogConfig = Path.GetFullPath(@"../../../" + "NLog.config");

            //loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            //NLog.LogManager.LoadConfiguration(loadLogConfig);

            //loggerFactory.ConfigureNLog(loadLogConfig);


            return serviceProvider;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog())
                    .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                    .AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
                    .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                    .AddTransient<ITelemetryRepository, TelemetryRepository>();

        }
    }
}
