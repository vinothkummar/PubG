﻿using Fanview.API.Clients;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
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
                                                         .AddSingleton<IConfiguration>(_configuration)
                                                         .AddSingleton<IHttpClientRequest, HttpClientRequest>()
                                                         .AddSingleton<IClientBuilder, ClientBuilder>()
                                                         .AddSingleton<IMongoDbClient, MongoDbClient>()
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
                                                         .AddSingleton<IMatchManagementRepository, MatchManagementRepository>()
                                                         .AddTransient<IMatchRepository, MatchRepository>()
                                                         .AddTransient<ITeamLiveStatusRepository, TeamLiveStatusRepository>()
                                                         .AddTransient<IAssetsRepository, AssetsRepository>()
                                                         .AddTransient<ILiveRepository,LiveRepository>()
                                                         .AddMemoryCache()
                                                         .AddSingleton<IDistributedCache>(cn => new RedisCache(new RedisCacheOptions
                                                         {
                                                             Configuration = "127.0.0.1:6379,abortConnect=false,connectTimeout=3000,responseTimeout=3000,syncTimeout=3000",
                                                             InstanceName = "FanviewCaching"
                                                         }))
                                                         //.AddDistributedRedisCache(options =>
                                                         //{
                                                         //    options.Configuration = "127.0.0.1:6379,abortConnect=False,connectTimeout=3000,responseTimeout=3000,syncTimeout=3000";
                                                         //    options.InstanceName = "Master";
                                                         //})
                                                         //.AddSingleton<RedisService>()
                                                         //.Configure<ApplicationSettings>(options => _configuration.GetSection("ApplicationSettings").Bind(options))                                                     
                                                         .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
