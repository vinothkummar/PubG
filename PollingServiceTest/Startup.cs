using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using FanviewPollingService.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

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
            services.AddTransient<IPlayerKillRepository, PlayerKillRepository>();
        }
    }
}
