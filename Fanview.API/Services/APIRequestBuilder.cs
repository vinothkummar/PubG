using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fanview.API.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Fanview.API.Services
{
    public class APIRequestBuilder : IAPIRequestBuilder
    {     

       //private IConfigurationRoot _configuration;
        private IConfiguration _Configuration;

        public APIRequestBuilder( IHostingEnvironment env, IConfiguration configuration)
        {

            //_configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();

            _Configuration = configuration;

        }

        public async Task<HttpClient> CreateRequestHeader()
        {
            var client = new HttpClient();            
            client.BaseAddress = new Uri(_Configuration["Logging:AppSettings:OrganizationUrl"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _Configuration["Logging:AppSettings:ApplicationKey"]);

            return client ;
        }

        
    }
}
