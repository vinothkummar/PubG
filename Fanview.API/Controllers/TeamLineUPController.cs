using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamLineUPController : ControllerBase
    {
        private ITeamRepository _teamRepository;

        public TeamLineUPController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        // GET: api/TeamLineUP/5
        [HttpGet("{teamId}", Name = "Get")]
        public Task<IEnumerable<TeamLineUp>> Get(string teamId)
        {
            return _teamRepository.GetTeamLine(teamId);
        }
    }
}
