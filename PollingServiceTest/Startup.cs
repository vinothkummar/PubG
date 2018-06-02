using FanviewPollingService.Contracts;
using FanviewPollingService.Repository;
using FanviewPollingService.Repository.Interfaces;
using FanviewPollingService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PollingServiceTest
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IHttpClientRequest, HttpClientRequest>();
            services.AddSingleton<IHttpClientBuilder, HttpClientBuilder>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ITelemetryRepository, TelemetryRepository>();
        }
    }
}
