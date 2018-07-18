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



namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        private ILiveRepository _liveRepository;
        private ILiveStats _liveStatus;

        public LiveController(ILiveRepository liveRepository, ILiveStats liveStatus)
        {
            _liveRepository = liveRepository;
            _liveStatus = liveStatus;
        }

        /// <summary>
        /// Returns Live Team Status
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving the Static Information
        /// Sample request: api/Live/Status/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>675619e6-4a11-6b92-cf2e-4c82428b78ef</param>
        [HttpGet("Status/{matchId}", Name = "GetLiveStatus")]
        public Task<LiveStatus> GetLiveStatus(string matchId)
        {
            // return _liveRepository.GetLiveStatus(matchId);

            return _liveStatus.GetLiveStatus(matchId);
        }


        /// <summary>
        /// Returns Live Damage List
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving the Static Information
        /// Sample request: api/Live/DamageList/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("DamageList/{matchId}", Name = "GetLiveDamageList")]
        public Task<LiveDamageList> GetLiveDamageList(string matchId)
        {
            return _liveRepository.GetLiveDamageList(matchId);
        }

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
        public Task<LiveKillList> GetLiveKillList(string matchId)
        {
            return _liveRepository.GetLiveKillList(matchId);
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
        public Task<LivePlayerStats> GetLivePlayerStats(string matchId)
        {
            return _liveRepository.GetLivePlayerStats(matchId);
        }
    }
}