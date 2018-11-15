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
using Microsoft.Extensions.Logging;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderListController : Controller
    {
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerKilled _playerKilled;
        private ILogger<LeaderListController> _logger;

        public LeaderListController(IPlayerKillRepository playerKillRepository, IPlayerKilled playerKilled,
                              ILogger<LeaderListController> logger)
        {
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
            _logger = logger;
        }

        /// <summary>
        /// Return Kill Leadear Board List     
        /// </summary>
        /// <remarks>
        /// Sample request: api/LeaderList/Kills{matchId}
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("Kills/{matchId}")]
        public Task<KillLeader> GetKillLeaderList(int matchId)
        {
           return _playerKillRepository.GetKillLeaderList(matchId,0);

        }


        /// <summary>
        /// Return top(n) kill list per matchId      
        /// </summary>
        /// <remarks>
        /// Sample request: api/LeaderList/Kills/{matchId}/4        
        /// </remarks>
        /// <param name='matchId'>1</param>
        /// <param name='topCount'>6</param>
        [HttpGet("Kills/{matchId}/{topCount}")]
        public Task<KillLeader> GetKillLeaderList(int matchId, int topCount)
        {
            return _playerKillRepository.GetKillLeaderList(matchId, topCount);

        }

        ///// <summary>
        ///// Returns Kill Location
        ///// </summary>
        ///// <remarks>
        ///// Sample request: api/Kill/Location/{matchId}     
        ///// </remarks>
        ///// <param name='matchId'>1</param>
        //[HttpGet("Location/{matchId}")]
        //public Task<IEnumerable<KillZone>> GetKillZone(int matchId)
        //{
        //    return _playerKillRepository.GetKillZone(matchId);
        //}

        /// <summary>
        /// Return Kill Leader List Per Tournament     
        /// </summary>
        /// <remarks>
        /// Sample request: api/Kills/LeaderList      
        /// </remarks>       
        [HttpGet("Kills")]
        public Task<KillLeader> GetKillLeaderList()
        {
            return _playerKillRepository.GetKillLeaderList();

        }


        /// <summary>
        /// Return Kill Leader List Topped by Time in the Tournament   
        /// </summary>
        /// <remarks>
        /// Sample request: api/LeaderList/TimeSurvived      
        /// </remarks>    
        [HttpGet("TimeSurvived")]
        public Task<KillLeader> GetKillLeaderListTopByTimed()
        {
            return _playerKillRepository.GetKillLeaderListTopByTimed();

        }

        /// <summary>
        /// Return Kill Leader List Topped by DamageDealt in the Tournament   
        /// </summary>
        /// <remarks>
        /// Sample request: api/LeaderList/DamageDealt      
        /// </remarks>    
        [HttpGet("DamageDealt")]
        public Task<KillLeader> GetKillLeaderListToppedByDamageDealt()
        {
            return _playerKillRepository.GetKillLeaderListToppedByDamageDealt();
        }
    }
}
