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
    [Route("api/[controller]")]
    [ApiController]
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
        /// <summary>
        /// Returns Player Killed for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: api/Kill/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("PlayerKilled/{matchId}")]
        public IEnumerable<Kill> GetPlayerKilled(string matchId)
        {
            var result = _playerKillRepository.GetPlayerKilled(matchId);
            return result.Result;
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Killiprinter text for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/All/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("KilliprinterText/{matchId}/All", Name = "GetAllKilliprinterForGraphics")]
        public IEnumerable<string> GetAllKilliprinterTextForGraphics(string matchId)
        {
            return _playerKilled.GetPlayerKilledText(matchId);
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Last 4 Killiprinter text for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/All/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("KilliprinterText/{matchId}/Last4", Name = "GetLast4KilliprinterForGraphics")]
        public IEnumerable<string> GetLast4KilliprinterTextForGraphics(string matchId)
        {
            return _playerKilled.GetLast4PlayerKilledText(matchId);
        }
    }
}
