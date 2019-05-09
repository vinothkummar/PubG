using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        private readonly ILiveStats _liveStats;
        private readonly IPlayerKillRepository _playerKillRepository;
        private readonly IPlayerKilled _playerKilled;
        private readonly ILiveRepository _liveRepository;

        public LiveController(
            ILiveStats liveStats,
            IPlayerKilled playerKilled,
            IPlayerKillRepository playerKillRepository,
            ILiveRepository liveRepository)
        {

            _liveStats = liveStats;
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
            _liveRepository = liveRepository;
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Killiprinter JSON for the given Match Id on the match live     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/All
        /// </remarks>       
        [HttpGet("Killiprinter", Name = "GetAllKilliprinterForGraphics")]
        public Task<IEnumerable<KilliPrinter>> GetAllKilliprinterForGraphics()
        { 
            return _playerKilled.GetLivePlayerKilled();
        }

        /// <summary>
        /// Returns Live Team Status
        /// </summary>               
        [HttpGet("Status", Name = "GetLiveStatus")]
        public Task<IEnumerable<LiveMatchStatus>> GetLiveStatus()
        {
            return _liveStats.GetLiveStatus();
        }

        /// <summary>
        /// Returns Live Damage List
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving the Static Information
        /// Sample request: api/Live/DamageList/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        [HttpGet("DamageList", Name = "GetLiveDamageList")]
        public Task<LiveDamageList> GetLiveDamageList()
        {
            return _liveRepository.GetLiveDamageList();
        }

        /// <summary>
        /// Returns Cumulative Damage List
        /// </summary>
        /// <remarks>     
        /// Sample request: api/live/CumulativeDamageList
        /// </remarks>       
        [HttpGet("CumulativeDamageList", Name = "GetTotalDamage")]
        public Task<object> GetTotalDamage()
        {
           return _playerKillRepository.GetTotalDamage();

        }


        /// <summary>
        /// Returns Live Kill List
        /// </summary>
        /// <remarks>     
        /// Sample request: api/live/KillList
        /// </remarks>       
        [HttpGet("KillList", Name = "GetLiveKillList")]
        public Task<KillLeader> GetLiveKillList()
        {
            return _playerKillRepository.GetLiveKillList(64);
        }
        [HttpDelete("Killiprinter", Name = "DeleteAllEventKillTable")]
        public void DeleteLiveEventKillTable()
        {
                _liveRepository.DeleteAllEventKillTable();
        }
        [HttpDelete("Status", Name = "DeleteTeamLiveStatesTable")]
        public void DeleteTeamLiveStatesTable()
        {
            _liveRepository.DeleteAllTeamStates();
        }
        [HttpDelete("MatchStatus", Name = "DeleteLiveMatchstat")]
        public void DeleteLiveMatchstat()
        {
            _liveRepository.DeleteEventMatchStates();
        }
        [HttpDelete("Damage", Name = "DeleteLiveDamage")]
        public void DeleteLiveDamage()
        {
            _liveRepository.DeleteEventLiveMatchDamage();
        }
        [HttpDelete("DeleteAll", Name = "DeleteAll")]
        public void DeleteAll()
        {
            _liveRepository.DeleteAll();
        }

        /// <summary>
        /// Returns Live Kill List for top n rows
        /// </summary>
        /// <remarks>        
        /// Sample request: api/live/KillList/{matchId} 
        /// </remarks>       
        /// <param name='topN'>top 6 rows</param>
        [HttpGet("KillList/{topN}")]
        public Task<KillLeader> GetLiveKillList(int topN)
        {
            return _playerKillRepository.GetLiveKillList(topN);
        }

        /// <summary>
        /// Returns Cumulative Kill List
        /// </summary>
        /// <remarks>     
        /// Sample request: api/live/CumulativeKillList
        /// </remarks>       

        [HttpGet("CumulativeKillList", Name = "TotalKillLists")]
        public Task<object> TotalKillLists()
        {
            return _playerKillRepository.GetTotalKills();
        }
        ///// <summary>
        ///// Returns Live Player Stats
        ///// </summary>
        ///// <remarks>              
        ///// Sample request: api/Live/PlayerStats/{matchId}
        ///// </remarks>        
        //[HttpGet("LivePlayerStats/{matchId}", Name = "GetLivePlayerStats")]
        //public Task<IEnumerable<PlayerProfileTournament>> GetLivePlayerStats(int matchId)
        //{
        //    var playerId1 = 1;
        //    return _teamPlayerRepository.GetTeamPlayersTournament(playerId1, matchId);
        //    //return _liveRepository.GetLivePlayerStats(matchId);

        //}

        [HttpGet("Ranking", Name = "GetLiveRanking")]
        public Task<IEnumerable<LiveTeamRanking>> GetLiveRanking()
        {
            return _liveStats.GetLiveRanking();
        }
        [HttpGet("CumulativeRanking", Name = "GetTotalRanking")]
        public Task<List<LiveTeamRanking>> GetTotalRanking()
        {
            return _liveStats.TotalRank();
        }

        [HttpGet("MatchStatus", Name = "GetMatchStatus")]
        public Task<EventLiveMatchStatus> GetMatchStatus()
        {
            return _liveStats.GetLiveMatchStatus();
        }
    }
}