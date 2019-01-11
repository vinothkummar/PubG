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

            services.AddScoped<IHttpClientRequest, HttpClientRequest>();
            services.AddScoped<IClientBuilder, ClientBuilder>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped(typeof(IAPIRequestBuilder), typeof(APIRequestBuilder));
            services.AddScoped(typeof(IServiceRequest), typeof(ServiceRequest));
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped(typeof(IMatchRepository), typeof(MatchRepository));
            services.AddScoped(typeof(ITelemetryRepository), typeof(TelemetryRepository));
            services.AddScoped(typeof(IPlayerKillRepository), typeof(PlayerKillRepository));
            services.AddScoped(typeof(IPlayerRepository), typeof(PlayerRepository));
            services.AddScoped(typeof(ITakeDamageRepository), typeof(TakeDamageRepository));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IPlayerKilled), typeof(PlayerKilled));
            services.AddScoped(typeof(IMatchSummaryRepository), typeof(MatchSummaryRepository));
            services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddScoped(typeof(ITeamPlayerRepository), typeof(TeamPlayerRepository));
            services.AddScoped(typeof(IEventScheduleRepository), typeof(EventScheduleRepository));
            services.AddScoped(typeof(IRanking), typeof(Ranking));
            services.AddScoped<IReadAssets, ReadAssets>();
            services.AddScoped<ITeamStats, TeamStats>();
            services.AddScoped(typeof(ILiveRepository), typeof(LiveRepository));
            services.AddScoped<ILiveStats, LiveStats>();
            services.AddScoped<IMatchManagementRepository, MatchManagementRepository>();
            services.AddScoped<ITeamLiveStatusRepository, TeamLiveStatusRepository>();
            services.AddScoped<IAssetsRepository, AssetsRepository>();
            services.AddSingleton<IDistributedCache>(serviceProvider => new RedisCache(new RedisCacheOptions {
                Configuration = "127.0.0.1:6379,abortConnect=false,connectTimeout=3000,responseTimeout=3000,syncTimeout=3000",
                InstanceName =  "FanviewCaching"
            }));
            services.AddScoped<ITeamRankingService, TeamRankingService>();
        }
    }
}
