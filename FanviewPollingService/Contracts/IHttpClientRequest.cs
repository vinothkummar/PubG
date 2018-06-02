using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FanviewPollingService.Contracts
{
    public interface IHttpClientRequest
    {
        Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string query);
    }
}
