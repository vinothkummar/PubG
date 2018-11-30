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
                                                         .AddSingleton<IEventRepository, EventRepository>()
                                                         .AddTransient<ICacheService, CacheService>()
                                                         .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                                                         .AddTransient<IPlayerKillRepository, PlayerKillRepository>()
                                                         .AddTransient<IPlayerRepository, PlayerRepository>()
                                                         .AddTransient<ITakeDamageRepository, TakeDamageRepository>()
                                                         .AddTransient<ITelemetryRepository,TelemetryRepository>()
                                                         .AddTransient<ITeamPlayerRepository, TeamPlayerRepository>()
                                                         .AddTransient<ITeamRepository, TeamRepository>()
                                                         .AddTransient<IMatchRepository, MatchRepository>()
                                                         .AddTransient<IMatchSummaryRepository, MatchSummaryRepository>()
                                                         .AddTransient<IMatchRepository, MatchRepository>()
                                                         .AddTransient<ITeamLiveStatusRepository, TeamLiveStatusRepository>()
                                                         .AddDistributedRedisCache(options =>
                                                         {
                                                             options.Configuration = "127.0.0.1:6379";
                                                             options.InstanceName = "Master";
                                                             
                                                         })
                                                         .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
