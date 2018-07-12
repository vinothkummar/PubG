using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;



namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        private ILiveRepository _liveRepository;

        public LiveController(ILiveRepository liveRepository)
        {
            _liveRepository = liveRepository;
        }

        /// <summary>
        /// Returns Live Team Status
        /// </summary>
        /// <remarks>
        /// This Api Currently Serving the Static Information
        /// Sample request: api/Status/{matchId}          
        /// Input Parameter: f84d39a1-8218-4438-9bf5-7150f9e0f093
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Status/{matchId}", Name = "GetLiveStatus")]
        public Task<LiveStatus> GetLiveStatus(string matchId)
        {
            return _liveRepository.GetLiveStatus(matchId);
        }

    }
}