using Fanview.API.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanview.API.Clients
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection _formatters;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(IConfiguration config, ILogger<ApiClient> logger)
        {
            var baseUrl = config["PubGApi:BaseUrl"];
            var token = config["PubGApi:AccessToken"];

            _logger = logger;
            _formatters = new MediaTypeFormatterCollection();
            _formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<string> GetMatchSummary(string matchId)
        {
            var url = $"/shards/pc-tournaments/matches/{matchId}";
            var req = await _httpClient.GetAsync(url).ConfigureAwait(false);
            if (!req.IsSuccessStatusCode)
            {
                var errorMsg = string.Format("Got %d when calling %s", (int)req.StatusCode, url);
                _logger.LogError(errorMsg);
                throw new ApiClientException(errorMsg);
            }
            return await req.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
