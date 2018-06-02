using System;
using Xunit;
using Fanview.API.Controllers;
using System.Diagnostics;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fanview.Test
{
    public class ControllerIntegrationTest
    {
   
        private IMatchRepository _matchRepository;
        private TestServer _server;
        private readonly HttpClient _client;

        public ControllerIntegrationTest()
        {
            
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(@"../../../"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var Configuration = config.Build();

            _server = new TestServer(new WebHostBuilder()
                      .UseConfiguration(Configuration)
                      .UseStartup<TestStartup>()
                      );
            _client = _server.CreateClient();         

            

        }
        [Fact]
        public void Assert_The_Response_Time_Pubg_Match_Endpoint_Request()
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            
            // Act
            var clientResponse = _client.GetAsync("/api/Match/1").Result;

            var jsonResult = clientResponse.Content.ReadAsStringAsync().Result;

            var jsonResultConvertedToJObjectObject = JsonConvert.DeserializeObject<JObject>(jsonResult);

            stopWatch.Stop();

            // Assert
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);
            

            var actual = TimeSpan.Parse(elapsedTime);            

            var expected = TimeSpan.Parse("00:00:00:30");

            Assert.True(expected > actual);

        }
    }
}
