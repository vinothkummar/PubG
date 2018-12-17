using Fanview.API.BusinessLayer;
using Fanview.API.BusinessLayer.Contracts;
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

            var path = Directory.GetCurrentDirectory();

            var _configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", true, true).Build();

            services.AddSingleton<IHttpClientRequest, HttpClientRequest>();
            services.AddSingleton<IClientBuilder, ClientBuilder>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton(typeof(IAPIRequestBuilder), typeof(APIRequestBuilder));
            services.AddSingleton(typeof(IServiceRequest), typeof(ServiceRequest));
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient(typeof(IMatchRepository), typeof(MatchRepository));
            services.AddTransient(typeof(ITelemetryRepository), typeof(TelemetryRepository));
            services.AddTransient(typeof(IPlayerKillRepository), typeof(PlayerKillRepository));
            services.AddTransient(typeof(IPlayerRepository), typeof(PlayerRepository));
            services.AddTransient(typeof(ITakeDamageRepository), typeof(TakeDamageRepository));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IPlayerKilled), typeof(PlayerKilled));
            services.AddTransient(typeof(IMatchSummaryRepository), typeof(MatchSummaryRepository));
            services.AddTransient(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddTransient(typeof(ITeamPlayerRepository), typeof(TeamPlayerRepository));
            services.AddSingleton(typeof(IEventScheduleRepository), typeof(EventScheduleRepository));
            services.AddTransient(typeof(IRanking), typeof(Ranking));
            services.AddSingleton<IReadAssets, ReadAssets>();
            services.AddTransient<ITeamStats, TeamStats>();
            services.AddSingleton(typeof(ILiveRepository), typeof(LiveRepository));
            services.AddTransient<ILiveStats, LiveStats>();
            services.AddTransient<IMatchManagementRepository, MatchManagementRepository>();
            services.AddTransient<ITeamLiveStatusRepository, TeamLiveStatusRepository>();
            services.AddTransient<IAssetsRepository, AssetsRepository>();
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions {
                Configuration = "127.0.0.1:6379,abortConnect=false,connectTimeout=3000,responseTimeout=3000,syncTimeout=3000",
                InstanceName =  "FanviewCaching"
            }));
        }
    }
}
