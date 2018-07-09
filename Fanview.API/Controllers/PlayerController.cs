using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using Fanview.API.Repository;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private ITeamPlayerRepository _teamPlayerRepository;

        public PlayerController(ITeamPlayerRepository teamPlayerRepository)
        {
            _teamPlayerRepository = teamPlayerRepository;
            
        }


        //// POST: api/Player
        //[HttpPost("Create")]
        //public void Post([FromBody] TeamPlayer value)
        //{
        //    _teamPlayerRepository.InsertTeamPlayer(value);
        //}
    }
}
