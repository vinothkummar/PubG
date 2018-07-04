using Fanview.API.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanview.API.Services
{
    public class ClientBuilder : IClientBuilder
    {
        private IConfiguration _configuration;

        public ClientBuilder()
        {
            var path = Directory.GetCurrentDirectory();
            _configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", true, true).Build();
            
        }
        public async Task<HttpClient> CreateRequestHeader()
        {
            var client = new HttpClient();            
            client.BaseAddress = new Uri(this._configuration["Logging:AppSettings:OrganizationUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);
            //client.Timeout = TimeSpan.FromMilliseconds(500);

            return await Task.FromResult(client);
        }

        public async Task<HttpClient> CreateRequestHeader(string baseUrl)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);
            //client.Timeout = TimeSpan.FromMilliseconds(500);

            return await Task.FromResult(client);
        }
    }
}
