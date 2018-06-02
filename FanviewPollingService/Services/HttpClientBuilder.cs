using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FanviewPollingService.Contracts;
using Microsoft.Extensions.Configuration;

namespace FanviewPollingService.Services
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private IConfiguration _configuration;

        public HttpClientBuilder()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            
        }
        public async Task<HttpClient> CreateRequestHeader()
        {
            var client = new HttpClient();            
            client.BaseAddress = new Uri(this._configuration["Logging:AppSettings:OrganizationUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);
            //client.Timeout = TimeSpan.FromMilliseconds(500);

            return client;
        }
    }
}
