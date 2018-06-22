using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace FanviewEventPollingService.Services
{
    public static class ServiceConfiguration
    {
       

        public static IServiceProvider BuildDI()
        {
            

            var serviceProvider = new ServiceCollection().AddLogging(configure => configure.AddSerilog())
                                                         .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                                                         .AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
                                                         .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))                                                           
                                                         .AddTransient<IPlayerKillRepository, PlayerKillRepository>()
                                                         .AddTransient<ITakeDamageRepository, TakeDamageRepository>()
                                                         .AddTransient<ITelemetryRepository,TelemetryRepository>()
                                                         .AddTransient<IMatchRepository, MatchRepository>()
                                                         .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
