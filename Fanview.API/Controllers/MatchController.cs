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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IMatchRepository _matchRepository;


        public MatchController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;           
        }

        //// GET: api/Match
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Match/5
        [HttpGet("ById/{id}", Name = "GetMatch")]        
        public Task<JObject> GetMatch(string id)
        {
            var result = _matchRepository.GetMatchesDetailsByID(id);
            return result;
           
        }
        
        //// POST: api/Match
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        
        //// PUT: api/Match/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
