﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;
using Fanview.API.Utility;
using Fanview.API.BusinessLayer.Contracts;
using Microsoft.Extensions.Logging;

namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class KillController : Controller
    {
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerKilled _playerKilled;
        private ILogger<KillController> _logger;

        public KillController(IPlayerKillRepository playerKillRepository, IPlayerKilled playerKilled,
                              ILogger<KillController> logger)
        {
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
            _logger = logger;
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
        /// Returns Killiprinter Text Message for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/All/Text          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Killiprinter/{matchId}/All/Text", Name = "GetAllKilliprinterTextForGraphics")]
        public IEnumerable<string> GetAllKilliprinterTextForGraphics(string matchId)
        {
            _logger.LogInformation("Get All KilliPrinter Text For Graphics Funtion Call Started" + Environment.NewLine);
            try
            {
                _logger.LogInformation("Get All KilliPrinter Text For Graphics Funtion Call Completed" + Environment.NewLine);

                return _playerKilled.GetPlayerKilledText(matchId);

                
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetAllKilliprinterTextForGraphics");

                throw;
            }
          
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Last 4 Killiprinter Text Message for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/ Last4/Text         
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Killiprinter/{matchId}/Last4/Text", Name = "GetLast4KilliprinterTextForGraphics")]
        public IEnumerable<string> GetLast4KilliprinterTextForGraphics(string matchId)
        {
            return _playerKilled.GetLast4PlayerKilledText(matchId);
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Killiprinter JSON for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/All         
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Killiprinter/{matchId}/All", Name = "GetAllKilliprinterForGraphics")]
        public IEnumerable<KilliPrinter> GetAllKilliprinterForGraphics(string matchId)
        {
            return _playerKilled.GetPlayerKilled(matchId);
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Last 4 Killiprinter JSON for the given Match Id     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/ Last4/Text         
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Killiprinter/{matchId}/Last4", Name = "GetLast4KilliprinterForGraphics")]
        public IEnumerable<KilliPrinter> GetLast4KilliprinterForGraphics(string matchId)
        {
            return _playerKilled.GetPlayerKilled(matchId).TakeLast(4);
        }
    }
}