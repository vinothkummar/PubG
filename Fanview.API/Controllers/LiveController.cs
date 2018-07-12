using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveController : ControllerBase
    {
        // GET: api/Live
        /// <summary>
        /// Returns Live Team status      
        /// </summary>
        /// <remarks>
        /// Sample request: api/Live/Status          
        /// Read live status from a Match Id Parameter.      
        /// </remarks>
        /// <param name='matchId'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        [HttpGet("Status/{matchId}", Name = "GetLiveStatus")]
        public LiveStatus GetLiveStatus(int matchId)
        {
            return null;
        }
    }
}