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
    [Route("api/Kill")]
    public class KillController : Controller
    {
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerKilled _playerKilled;

        public KillController(IPlayerKillRepository playerKillRepository, IPlayerKilled playerKilled )
        {
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
        }

        // GET: api/Telemetry
        [HttpGet("PlayerKilled")]
        public IEnumerable<Kill> GetPlayerKilled()
        {
            var result = _playerKillRepository.GetPlayerKilled();

            return result.Result.Count() > 0 ? result.Result: null;           
        }

        [HttpGet("PlayerKilled/Media")]
        public IEnumerable<string> GetPlayerKilledForMediaStream()
        {
            return _playerKilled.GetPlayerKilled();
        }

        [HttpGet("PlayerKilled/Media/Last4")]
        public IEnumerable<string> GetLast4PlayerKilledForMediaStream()
        {
            return _playerKilled.GetLast4PlayerKilled();
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
