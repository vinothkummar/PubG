using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;
using Fanview.API.Utility;
using Fanview.API.BusinessLayer.Contracts;



namespace Fanview.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class KillController : Controller
    {
        private IPlayerKillRepository _playerKillRepository;
        private IPlayerKilled _playerKilled;

        public KillController(IPlayerKillRepository playerKillRepository, IPlayerKilled playerKilled )
        {
            _playerKillRepository = playerKillRepository;
            _playerKilled = playerKilled;
        }

        // GET: api/Telemetry
        [HttpGet("PlayerKilled")]
        public IEnumerable<Kill> GetPlayerKilled()
        {
            var result = _playerKillRepository.GetPlayerKilled();
            return result.Result;
        }

        //[HttpGet("Killiprinter/All", Name = "GetAllKilliprinterForGraphics")]
        //public IEnumerable<string> GetAllKilliprinterForGraphics()
        //{
        //    return _playerKilled.GetPlayerKilled();
        //}

        //[HttpGet("Killiprinter/All", Name = "GetLast4KilliprinterForGraphics")]
        //public IEnumerable<string> GetLast4KilliprinterForGraphics()
        //{
        //    return _playerKilled.GetLast4PlayerKilled();
        //}
    }
}
