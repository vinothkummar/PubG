using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Services.Interface
{
    public interface IAPIRequestBuilder
    {        
        Task<HttpClient> CreateRequestHeader();
    }

    
}
