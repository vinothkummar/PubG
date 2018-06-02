using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private IAPIRequestBuilder _apiRequestBuilder;
        private IServiceRequest _serviceRequest;

        public MatchRepository(IAPIRequestBuilder apiRequestBuilder,IServiceRequest serviceRequest)
        {
            _apiRequestBuilder = apiRequestBuilder;
            _serviceRequest = serviceRequest;
        }
        public async Task<JObject> GetMatchesByID(string id)
        {            
            var clientResponse = _serviceRequest.GetAsync(await _apiRequestBuilder.CreateRequestHeader(), "matches/"+id).Result;

            var jsonResult = clientResponse.Content.ReadAsStringAsync().Result;

            var jsonResultConvertedToJObjectObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            return await Task.FromResult(jsonResultConvertedToJObjectObject);
        }
    }
}
