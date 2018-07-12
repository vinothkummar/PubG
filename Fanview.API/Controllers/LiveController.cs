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
        [HttpGet("Status", Name = "GetLiveStatus")]
        public LiveStatus GetLiveStatus(int matchId)
        {
            return null;
        }
    }
}