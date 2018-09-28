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
    public class OpenAPIController : ControllerBase
    {
        private IRanking _ranking;

        public OpenAPIController(IRanking ranking)
        {
            _ranking = ranking;
        }

        /// <summary>
        /// Poll Data on the Open Api from the Match Round Status and the Telemetry 
        /// To Calculate the team standing on the Match Round
        /// </summary>
        /// <remarks>
        /// Sample request: RoundRankingData/{matchId}         
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpPost("RoundRankingData/{matchId}", Name = "PostAndCalculateMatchRoundRanking")]
        public async Task<IEnumerable<MatchRanking>> PostAndCalculateMatchRoundRanking(string matchId)
        {
            return await _ranking.PollAndGetMatchRanking(matchId);
        }
    }
}