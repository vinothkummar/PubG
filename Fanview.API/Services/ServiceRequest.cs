using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fanview.API.Services.Interface;

namespace Fanview.API.Services
{
    public class ServiceRequest : IServiceRequest
    {
        public Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string query)
        {
            var response = new HttpResponseMessage();
            try
            {
                response = httpClient.GetAsync(query).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(response);
        }
    }
}
