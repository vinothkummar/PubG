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
    }
}
