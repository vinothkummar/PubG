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

        /// <summary>
        /// Returns Team MatchUp Response for the given teamId1 And teamId2     
        /// </summary>
        /// <remarks>
        /// Sample request: MatchUp/{playerId1}/And/{playerId2}
        /// </remarks>
        /// <param name='teamId1'>5b369085a510862ec07c824a</param>
        /// <param name='teamId2'>5b36912ca510862ec07c8251</param>
        [HttpGet("MatchUp/{teamId1}/And/{teamId2}", Name = "GetTeamMatchup")]
        public Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2)
        {
            return _teamRepository.GetTeamMatchup(teamId1, teamId2);
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
