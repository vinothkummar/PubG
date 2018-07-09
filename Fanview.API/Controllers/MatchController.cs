﻿using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private IMatchRepository _matchRepository;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;

        public MatchController(IMatchRepository matchRepository, IMatchSummaryRepository matchSummaryRepository, IPlayerKillRepository playerKillRepository)
        {
            _matchRepository = matchRepository;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
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
    }
}
