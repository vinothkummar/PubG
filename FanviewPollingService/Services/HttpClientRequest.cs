using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FanviewPollingService.Contracts;

namespace FanviewPollingService.Services
{
    public class HttpClientRequest : IHttpClientRequest
    {
        public async Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string query)
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
            return await Task.FromResult(response);
        }
    }
}
