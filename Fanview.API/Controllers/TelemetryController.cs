using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;
using Fanview.API.Utility;
using Fanview.API.BusinessLayer.Contracts;



namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Telemetry")]
    public class TelemetryController : Controller
    {
        private ITelemetryRepository _telemetryApiRepository;
        private IPlayerKilled _playerKilled;

        public TelemetryController(ITelemetryRepository telemetryApiRepository, IPlayerKilled playerKilled )
        {
            _telemetryApiRepository = telemetryApiRepository;
            _playerKilled = playerKilled;
        }

        // GET: api/Telemetry
        [HttpGet("PlayerKilled")]
        public IEnumerable<PlayerKill> GetPlayerKilled()
        {
            var result = _telemetryApiRepository.GetPlayerKills();


            return result.Result.Count() > 0 ? result.Result: null;           
        }

        [HttpGet("PlayerKilledMediaText")]
        public IEnumerable<string> GetPlayerKilledForMediaStream()
        {
            return _playerKilled.GetPlayerKilled();
        }

        // GET: api/Telemetry/5
        [HttpGet("PlayerKill/{id}", Name = "GetTelemetry")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Telemetry
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Telemetry/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
