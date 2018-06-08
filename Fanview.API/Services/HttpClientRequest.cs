using Fanview.API.Services.Interface;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Services
{
    public class HttpClientRequest : IHttpClientRequest
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
