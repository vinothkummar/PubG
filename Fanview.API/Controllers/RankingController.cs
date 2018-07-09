using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private IRanking _ranking;

        public RankingController(IRanking ranking)
        {
            _ranking = ranking;
        }

        [HttpGet("TeamRank/{matchId}", Name = "GetMatchRanking")]
        public async Task<IEnumerable<MatchRanking>> GetMatchRanking(string matchId)
        {
             
            return await _ranking.GetMatchRanking(matchId);
        }
    }
}
