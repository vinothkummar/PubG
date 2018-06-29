using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;

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

        //// GET: api/Player
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Player/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Player
        [HttpPost("Create")]
        public void Post([FromBody] TeamPlayer value)
        {
            _teamPlayerRepository.InsertTeamPlayer(value);
        }

        //// PUT: api/Player/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
