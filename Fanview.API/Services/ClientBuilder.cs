using Fanview.API.Services.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
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

        public async Task<HttpClient> CreateLiveRequestHeader()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(this._configuration["Logging:AppSettings:LiveUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);

            return await Task.FromResult(client);
        }

        public async Task<HttpClient> CreateRequestHeader()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);            
            client.BaseAddress = new Uri(this._configuration["Logging:AppSettings:OrganizationUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));           
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);           

            return await Task.FromResult(client);
        }

        public async Task<HttpClient> CreateRequestHeader(string baseUrl)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseUrl);          
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._configuration["Logging:AppSettings:ApplicationKey"]);            

            return await Task.FromResult(client);
        }
    }
}
