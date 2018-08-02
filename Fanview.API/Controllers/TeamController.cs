using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        // GET: api/TeamLineUP/5
        /// <summary>
        ///Returns teamlineup       
        /// </summary>
        /// <remarks>
        /// Sample request: api/Team/LineUp/{teamId}   
        /// Input Parameters: 5b369085a510862ec07c824a ;  5b3e63846c0810ce58c299a5
        /// </remarks>
        /// <param name='teamId'>5b369085a510862ec07c824a</param>
        [HttpGet("LineUp/{teamId}", Name = "Get")]
        public Task<TeamLineUp> Get(int teamId)
        {
            return _teamRepository.GetTeamLine(teamId);
        }

        /// <summary>
        ///Returns TeamRoute       
        /// </summary>
        /// <remarks>
        /// Sample request: api/Team/Route
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("Route/{matchId}", Name = "GetTeamRoute")]
        public Task<IEnumerable<TeamRoute>> GetTeamRoute(int matchId)
        {
            return _teamRepository.GetTeamRoute(matchId);
        }


        /// <summary>
        /// Returns Team Landing     
        /// </summary>
        /// <remarks>
        /// Sample request: api/Team/Landing  
        /// This Api Currently Serving the Static Information
        /// Input Parameters: 5b369085a510862ec07c824a 
        /// </remarks>
        /// <param name='matchId'>5b369085a510862ec07c824a</param>
        [HttpGet("Landings/{matchId}", Name = "GetTeamLandings")]
        public async Task<TeamLanding> GetTeamLandings(int matchId)
        {
            var res= await _teamRepository.GetTeamLanding(matchId);
            return res;

        }

        //GET:api/Team
        /// <summary>
        /// Returns All Teams  
        /// </summary>
        /// <remarks>
        /// Sample request: GetAllTeam
        /// </remarks>
        [HttpGet("All", Name = "GetAllTeam")]
        public Task<IEnumerable<Team>> GetAllTeam()
        {
            return _teamRepository.GetAllTeam();
        }

        /// <summary>
        /// Returns Team Profile for the given teamId1     
        /// </summary>
        /// <remarks>
        /// Sample request: Profile/{teamId}
        /// Input parameters are 1 - 20;
        /// </remarks>
        /// <param name='teamId1'>1</param>       
        [HttpGet("Profile/{teamId1}", Name = "GetTeamProfile")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfile(string teamId1)
        {
            return _teamRepository.GetTeamProfile(teamId1);
        }

        [HttpGet("Profile/{teamId1}/{matchId}", Name = "GetTeamProfileByMatchId")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfileByMatchId(string teamId1, int matchId)
        {
            return _teamRepository.GetTeamProfileByMatchId(teamId1, matchId);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}/{matchId}", Name = "GetTeamProfilesByTeamIdAndMatchId")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId)
        {
            return _teamRepository.GetTeamProfilesByTeamIdAndMatchId(teamId1, teamId2, matchId);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}", Name = "GetTeamProfileMatchUp")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfileMatchUp(string teamId1, string teamId2)
        {
            return _teamRepository.GetTeamProfileMatchUp(teamId1, teamId2);
        }
    }
}
