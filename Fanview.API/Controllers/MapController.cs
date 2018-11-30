using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.BusinessLayer.Contracts;
using System.Collections.Generic;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;


namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private IMatchRepository _matchRepository;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerRepository _playerRepository;
        private ITeamRepository _teamRepository;
        private ITeamStats _teamStats;

        public MapController(IMatchRepository matchRepository,
                               IMatchSummaryRepository matchSummaryRepository,
                               IPlayerKillRepository playerKillRepository,
                               IPlayerRepository playerRepository,
                               ITeamRepository teamRepository,
                               ITeamStats teamStats
                               )
        {
            _matchRepository = matchRepository;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
            _playerRepository= playerRepository;
            _teamRepository = teamRepository;
            _teamStats = teamStats;
        }
        
        /// <summary>
        /// Returns Flight Path Info
        /// </summary>
        /// <remarks>      
        /// Sample request: api/Map/FlightPath/{matchId}
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("FlighPath/{matchId}", Name = "GetFlightPath")]
        public Task<FlightPath> GetFlightPath(int matchId)
        {
            return _playerRepository.GetFlightPath(matchId);
        }

        /// <summary>
        /// Returns SafeZone Circle Position
        /// </summary>
        /// <remarks>      
        /// Sample request: api/Map/SafeZone/{matchId}
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("SafeZone/{matchId}", Name = "GetMatchSafeZone")]
        public Task<object> GetMatchSafeZone(int matchId)
        {
            return _matchRepository.GetMatchSafeZone(matchId);
        }
      
        /// <summary>
        /// Returns Players Kill Location
        /// </summary>
        /// <remarks>
        /// Sample request: api/Map/KillLocation/{matchId}     
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("KillLocation/{matchId}")]
        public Task<object> GetKillZone(int matchId)
        {
            return _playerKillRepository.GetKillZone(matchId);
        }

        /// <summary>
        /// Returns Team Landing     
        /// </summary>
        /// <remarks>
        /// Sample request: api/Map/TeamLanding 
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("TeamLandings/{matchId}", Name = "GetTeamLandings")]
        public async Task<TeamLanding> GetTeamLandings(int matchId)
        {
            return await _teamRepository.GetTeamLanding(matchId);
        }

        /// <summary>
        /// Returns TeamRoute       
        /// </summary>
        /// <remarks>
        /// Sample request: api/Map/TeamRoute
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("TeamRoute/{matchId}", Name = "GetTeamRoute")]
        public Task<TeamRoute> GetTeamRoute(int matchId)
        {
            return _teamStats.GetTeamRoute(matchId);
        }
    }
}
