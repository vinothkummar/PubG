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
            var serviceProvider = new ServiceCollection().AddLogging(configure => configure.AddSerilog())
                                                         .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                                                         .AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
                                                         .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                                                         .AddTransient<ITelemetryRepository, TelemetryRepository>()
                                                         .BuildServiceProvider();
            return serviceProvider;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddSerilog());
                    //.AddSingleton<IHttpClientRequest, HttpClientRequest>()
                    //.AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
                    //.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                    //.AddTransient<ITelemetryRepository, TelemetryRepository>();

        }
    }
}
