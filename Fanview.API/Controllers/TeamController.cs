using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Newtonsoft.Json.Linq;

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

        ////// POST: api/Team
        //[HttpPost("Create")]
        //public void Post([FromBody] Team value)
        //{
        //    _teamRepository.InsertTeam(value);
        //}
    }
}
