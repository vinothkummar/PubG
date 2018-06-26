using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface IClientBuilder
    {
        Task<HttpClient> CreateRequestHeader();
    }
}
