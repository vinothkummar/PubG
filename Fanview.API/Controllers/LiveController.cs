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
        private ILiveRepository _liveRepository;
        private ILiveStats _liveStatus;
        private IRanking _ranking;
        private ITeamPlayerRepository _teamPlayerRepository;
        private ITeamRepository _teamRespository;
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerKilled _playerKilled;

        public LiveController(ILiveRepository liveRepository, ILiveStats liveStatus, IRanking ranking, IPlayerKilled playerKilled,
                              ITeamPlayerRepository teamPlayerRepository, ITeamRepository teamRepository, IPlayerKillRepository playerKillRepository)
        {

            _liveRepository = liveRepository;
            _liveStatus = liveStatus;
            _ranking = ranking;
            _teamPlayerRepository = teamPlayerRepository;
            _teamRespository = teamRepository;
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
        }

        // GET: api/Telemetry
        /// <summary>
        /// Returns Killiprinter JSON for the given Match Id on the match live     
        /// </summary>
        /// <remarks>
        /// Sample request: Killiprinter/{matchId}/All
        /// </remarks>       
        [HttpGet("Killiprinter_old", Name = "GetAllKilliprinterForGraphics")]
        public async Task<IEnumerable<KilliPrinter>> GetAllKilliprinterForGraphics()
        { 
            return await _playerKilled.GetLivePlayerKilled();
        }

        [HttpGet("Killiprinter", Name = "GetAllKilliprinterForGraphics2")]
        public Task<IEnumerable<KilliPrinter>> GetAllKilliprinterForGraphics2()
        {
            return  _playerKilled.GetLivePlayerKilledMongo();
        }


        
        /// <summary>
        /// Returns Live Team Status
        /// </summary>               
        [HttpGet("Status", Name = "GetLiveStatus")]
        public Task<IEnumerable<LiveMatchStatus>> GetLiveStatus2()
        {
            return _liveStatus.GetLiveStatusMongo();
        }


        ///// <summary>
        ///// Returns Live Damage List
        ///// </summary>
        ///// <remarks>
        ///// This Api Currently Serving the Static Information
        ///// Sample request: api/Live/DamageList/{matchId}          
        ///// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        ///// </remarks>
        ///// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        //[HttpGet("DamageList/{matchId}", Name = "GetLiveDamageList")]
        //public Task<LiveDamageList> GetLiveDamageList(string matchId)
        //{
        //    return _liveRepository.GetLiveDamageList(matchId);
        //}

        /// <summary>
        /// Returns Live Kill List
        /// </summary>
        /// <remarks>     
        /// Sample request: api/live/KillList/{matchId}
        /// </remarks>       
        [HttpGet("KillList", Name = "GetLiveKillList")]
        public Task<KillLeader> GetLiveKillList()
        {
            return _playerKillRepository.GetLiveKillList(0);
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
        /// Returns Live Player Stats
        /// </summary>
        /// <remarks>              
        /// Sample request: api/Live/PlayerStats/{matchId}
        /// </remarks>        
        //[HttpGet("LivePlayerStats/{matchId}", Name = "GetLivePlayerStats")]
        //public Task<IEnumerable<PlayerProfileTournament>> GetLivePlayerStats(int matchId)
        //{
        //    var playerId1 = 1;
        //    return _teamPlayerRepository.GetTeamPlayersTournament(playerId1, matchId);
        //    //return _liveRepository.GetLivePlayerStats(matchId);

        //}

        //[HttpGet("Ranking", Name = "GetLiveRanking")]
        //public Task<IEnumerable<LiveTeamRanking>> GetLiveRanking()
        //{
        //    return _liveStatus.GetLiveRanking();
        //}

        [HttpGet("MatchStatus", Name = "GetMatchStatus")]
        public Task<Object> GetMatchStatus()
        {
            return _liveStatus.GetLiveMatchStatus();
        }
    }
}