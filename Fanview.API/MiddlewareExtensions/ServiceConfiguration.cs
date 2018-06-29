using Fanview.API.BusinessLayer;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fanview.API.MiddlewareExtensions
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientRequest, HttpClientRequest>();
            services.AddSingleton<ClientBuilder, ClientBuilder>();
            services.AddSingleton(typeof(IAPIRequestBuilder), typeof(APIRequestBuilder));
            services.AddSingleton(typeof(IServiceRequest), typeof(ServiceRequest));
            services.AddSingleton(typeof(IMatchRepository), typeof(MatchRepository));
            services.AddTransient(typeof(ITelemetryRepository), typeof(TelemetryRepository));
            services.AddTransient(typeof(IPlayerKillRepository), typeof(PlayerKillRepository));
            services.AddTransient(typeof(ITakeDamageRepository), typeof(TakeDamageRepository));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IPlayerKilled), typeof(PlayerKilled));
            services.AddTransient(typeof(IMatchSummaryRepository), typeof(MatchSummaryRepository));
            services.AddTransient(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddTransient(typeof(ITeamPlayerRepository), typeof(TeamPlayerRepository));
        }
    }
}
