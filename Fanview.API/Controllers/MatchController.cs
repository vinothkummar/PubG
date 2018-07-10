using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.BusinessLayer.Contracts;
using System.Collections.Generic;


namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IMatchRepository _matchRepository;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private IRanking _ranking;

        public MatchController(IMatchRepository matchRepository,
                               IMatchSummaryRepository matchSummaryRepository,
                               IPlayerKillRepository playerKillRepository,
                               IRanking ranking
                               )
        {
            _matchRepository = matchRepository;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
            _ranking = ranking;
        }

        // GET: api/Match/5
        [HttpGet("ById/{id}", Name = "GetMatch")]
        public Task<JObject> GetMatch(string id)
        {
            var result = _matchRepository.GetMatchesDetailsByID(id);
            return result;
        }

        // POST: api/Match/PollSummary/{matchId}
        [HttpPost("PollSummary/{matchId}", Name = "PostSummary")]
        public void PostSummary(string matchId)
        {
            _matchSummaryRepository.PollMatchSummary(matchId);
        }

        // POST: api/Match/PollParticipantStats/{matchId}
        [HttpPost("PollParticipantStats/{matchId}", Name = "PostParticipantStats")]
        public void PostParticipantStats(string matchId)
        {
            _matchSummaryRepository.PollMatchParticipantStats(matchId);
        }

        // POST: api/Match
        [HttpPost("PollTelemetryPlayerKilled/{matchId}", Name = "PostPlayerKilled")]
        public void PostPlayerKilled(string matchId)
        {
            _playerKillRepository.PollTelemetryPlayerKilled(matchId);
        }

        // POST: api/Match
        [HttpPost("CreateDummyTeamPlayers/{matchId}", Name = "PostDummyTestTeamPlayers")]
        public void PostDummyTestTeamPlayers(string matchId)
        {
            _matchSummaryRepository.CreateAndMapTestTeamPlayerFromMatchHistory(matchId);
        }

        // POST: api/Match/PostRoundRankingData        
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
