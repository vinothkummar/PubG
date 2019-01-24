using Fanview.API.BusinessLayer;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Clients;
using Fanview.API.Model;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Fanview.API.MiddlewareExtensions
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientRequest, HttpClientRequest>();
            services.AddSingleton<IClientBuilder, ClientBuilder>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton(typeof(IMatchRepository), typeof(MatchRepository));
            services.AddSingleton(typeof(ITelemetryRepository), typeof(TelemetryRepository));
            services.AddSingleton(typeof(IPlayerKillRepository), typeof(PlayerKillRepository));
            services.AddSingleton(typeof(IPlayerRepository), typeof(PlayerRepository));
            services.AddSingleton(typeof(ITakeDamageRepository), typeof(TakeDamageRepository));
            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddSingleton(typeof(IPlayerKilled), typeof(PlayerKilled));
            services.AddSingleton(typeof(IMatchSummaryRepository), typeof(MatchSummaryRepository));
            services.AddSingleton(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddSingleton(typeof(ITeamPlayerRepository), typeof(TeamPlayerRepository));
            services.AddSingleton(typeof(IEventScheduleRepository), typeof(EventScheduleRepository));
            services.AddSingleton(typeof(IRanking), typeof(Ranking));
            services.AddSingleton<ITeamStats, TeamStats>();
            services.AddSingleton(typeof(ILiveRepository), typeof(LiveRepository));
            services.AddSingleton<ILiveStats, LiveStats>();
            services.AddSingleton<IMatchManagementRepository, MatchManagementRepository>();
            services.AddSingleton<ITeamLiveStatusRepository, TeamLiveStatusRepository>();
            services.AddSingleton<IAssetsRepository, AssetsRepository>();
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions {
                Configuration = "127.0.0.1:6379,abortConnect=false,connectTimeout=3000,responseTimeout=3000,syncTimeout=3000",
                InstanceName =  "FanviewCaching"
            }));
            services.AddSingleton<ITeamRankingService, TeamRankingService>();
            services.AddSingleton<IMongoDbClient, MongoDbClient>();
        }
    }
}
