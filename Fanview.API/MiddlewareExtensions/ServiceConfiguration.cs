using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using FanviewPollingService.Repository.Interfaces;
using FanviewPollingService.Repository;

namespace Fanview.API.MiddlewareExtensions
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {           
            services.AddSingleton(typeof(IAPIRequestBuilder), typeof(APIRequestBuilder));
            services.AddSingleton(typeof(IServiceRequest), typeof(ServiceRequest));
            services.AddSingleton(typeof(IMatchRepository), typeof(MatchRepository));
            services.AddTransient(typeof(ITelemetryApiRepository), typeof(TelemetryApiRepository));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
