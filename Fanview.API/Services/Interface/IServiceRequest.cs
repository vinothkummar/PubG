using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface IServiceRequest
    {
        Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string query);
    }
}
