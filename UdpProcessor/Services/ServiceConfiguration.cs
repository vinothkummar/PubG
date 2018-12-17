using Fanview.API.Model;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace Fanview.UDPProcessor.Services
{
    public static class ServiceConfiguration
    {
        
        public static IServiceProvider BuildDI()
        {
            var path = Directory.GetCurrentDirectory();

            var _configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", true, true).Build();
            
            var serviceProvider = new ServiceCollection().AddLogging(configure => configure.AddSerilog())
                                                         .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                                                         .AddSingleton<IClientBuilder, ClientBuilder>()
                                                         .AddSingleton<IEventRepository, EventRepository>()
                                                         .AddSingleton<IHostingEnvironment, HostingEnvironment>()
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
                                                         .AddTransient<IAssetsRepository, AssetsRepository>()
                                                         .AddDistributedRedisCache(options =>
                                                         {
                                                             options.Configuration = "13.66.243.193:6379,abortConnect=False";
                                                             options.InstanceName = "Master";
                                                         })
                                                         //.AddSingleton<RedisService>()
                                                         //.Configure<ApplicationSettings>(options => _configuration.GetSection("ApplicationSettings").Bind(options))                                                     
                                                         .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
