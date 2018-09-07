using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.BusinessLayer.Contracts;


namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private ITeamRepository _teamRepository;
        private ITeamStats _teamStats;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
           
        }

        // GET: api/TeamLineUP/5, 
        /// <summary>
        ///Returns Team lineup       
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
        /// Returns All Teams  
        /// </summary>       
        [HttpGet("All", Name = "GetAllTeam")]
        public Task<IEnumerable<Team>> GetAllTeam()
        {
            return _teamRepository.GetAllTeam();
        }

        /// <summary>
        /// Returns Team Stats for all the teams  
        /// </summary>
        /// <remarks>
        [HttpGet("Stats/Overall", Name = "GetAllTeamStats")]
        public Task<object> GetAllTeamStats()
        {
            return _teamRepository.GetAllTeamStats();
        }

        [HttpGet("Stats/{matchId}", Name = "GetTeamStats")]
        public Task<object> GetTeamStats(int matchId)
        {
            return _teamRepository.GetTeamStats(matchId);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}", Name = "GetTeamProfileMatchUp")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfileMatchUp(string teamId1, string teamId2)
        {
            return _teamRepository.GetTeamProfileMatchUp(teamId1, teamId2);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}/{matchId}", Name = "GetTeamProfilesByTeamIdAndMatchId")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId)
        {
            return _teamRepository.GetTeamProfilesByTeamIdAndMatchId(teamId1, teamId2, matchId);
        }
    }
}
