﻿using System;
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
    [ApiController]
    public class LiveController : ControllerBase
    {
        private ILiveRepository _liveRepository;
        private ILiveStats _liveStatus;
        private IRanking _ranking;
        private ITeamPlayerRepository _teamPlayerRepository;
        private ITeamRepository _teamRespository;
        private IPlayerKillRepository _playerKillRepository;

        public LiveController(ILiveRepository liveRepository, ILiveStats liveStatus, IRanking ranking,
                              ITeamPlayerRepository teamPlayerRepository, ITeamRepository teamRepository, IPlayerKillRepository playerKillRepository)
        {

            _liveRepository = liveRepository;
            _liveStatus = liveStatus;
            _ranking = ranking;
            _teamPlayerRepository = teamPlayerRepository;
            _teamRespository = teamRepository;
            _playerKillRepository = playerKillRepository;
        }

        ///// <summary>
        ///// Returns Live Team Status
        ///// </summary>
        ///// <remarks>
        ///// This Api Currently Serving the Static Information
        ///// Sample request: api/Live/Status/{matchId}          
        ///// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        ///// </remarks>
        ///// <param name='matchId'>675619e6-4a11-6b92-cf2e-4c82428b78ef</param>
        //[HttpGet("Status/{matchId}", Name = "GetLiveStatus")]
        //public Task<IEnumerable<LiveTeamPlayerStatus>> GetLiveStatus(int matchId)
        //{
        //    // return _liveRepository.GetLiveStatus(matchId);

        //    return _liveStatus.GetLiveStatus(matchId);
        //}


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
        /// This Api Currently Serving Static Information
        /// Sample request: api/live/KillList/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("KillList/{matchId}", Name = "GetLiveKillList")]
        public Task<KillLeader> GetLiveKillList(string matchId)
        {
            return _playerKillRepository.GetLiveKillList(matchId,0);
        }

        /// <summary>
        /// Returns Live Kill List for top n rows
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving Static Information
        /// Sample request: api/live/KillList/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        /// <param name='topN'>top 6 rows</param>
        [HttpGet("KillList/{matchId}/{topN}")]
        public Task<KillLeader> GetLiveKillList(string matchId, int topN)
        {
            return _playerKillRepository.GetLiveKillList(matchId,topN);
        }

        /// <summary>
        /// Returns Live Player Stats
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving the Static Information        
        /// Sample request: api/Live/PlayerStats/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("LivePlayerStats/{matchId}", Name = "GetLivePlayerStats")]
        public Task<IEnumerable<PlayerProfileTournament>> GetLivePlayerStats(int matchId)
        {
            var playerId1 = 1;
            return _teamPlayerRepository.GetTeamPlayersTournament(playerId1, matchId);
            //return _liveRepository.GetLivePlayerStats(matchId);

        }

        [HttpGet("Ranking/{matchId}", Name = "GetLiveRanking")]
        public Task<IEnumerable<MatchRanking>> GetLiveRanking(int matchId)
        {
            return _liveStatus.GetLiveStatsRanking(matchId);
        }


        ////dummy implementation to other to work
        //[HttpGet("LiveTeamStats/{matchId}", Name = "GetLiveTeamStats")]
        //public Task<IEnumerable<TeamRankingView>> GetLiveTeamStats(int matchId)
        //{
        //    var teamId1 = "0";

        //    return _teamRespository.GetTeamProfileByMatchId(teamId1, matchId);
        //}
    }
}