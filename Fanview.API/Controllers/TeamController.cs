using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public Task<IEnumerable<TeamLineUp>> Get(string teamId)
        {
            return _teamRepository.GetTeamLine(teamId);
        }
        [HttpGet("MatchUp/{teamId1}/And/{teamId2}", Name = "GetTeamMatchup")]
        public Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2)
        {
            return _teamRepository.GetTeamMatchup(teamId1, teamId2);
        }
        //GET:api/Team
        /// <summary>
        /// Returns All Teams  
        /// </summary>
        /// <remarks>
        /// Sample request: GetAllTeam
        /// </remarks>

        [HttpGet("Team", Name = "GetAllTeam")]
        public Task<IEnumerable<Team>> GetAllTeam()
        {
            return _teamRepository.GetAllTeam();
        }







        /// <summary>
        /// Returns Team Profile for the given teamId1     
        /// </summary>
        /// <remarks>
        /// Sample request: Profile/{teamId}
        /// </remarks>
        /// <param name='teamId1'>5b369085a510862ec07c824a</param>       
        [HttpGet("Profile/{teamId1}", Name = "GetTeamProfile")]
        public Task<TeamLineUp> GetTeamProfile(string teamId1)
        {
            return _teamRepository.GetTeamProfile(teamId1);
        }
    }
}
