using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.Extensions.DependencyInjection;
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
                                                         .AddSingleton<IClientBuilder, ClientBuilder>()
                                                         .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                                                         .AddTransient<IPlayerKillRepository, PlayerKillRepository>()
                                                         .AddTransient<IPlayerRepository, PlayerRepository>()
                                                         .AddTransient<ITakeDamageRepository, TakeDamageRepository>()
                                                         .AddTransient<ITelemetryRepository,TelemetryRepository>()
                                                         .BuildServiceProvider();
            return serviceProvider;
        }

        //public static void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddLogging(configure => configure.AddSerilog());
        //            //.AddSingleton<IHttpClientRequest, HttpClientRequest>()
        //            //.AddSingleton<IHttpClientBuilder, HttpClientBuilder>()
        //            //.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
        //            //.AddTransient<ITelemetryRepository, TelemetryRepository>();

        //}
    }
}
