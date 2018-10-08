using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;



namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MatchManagementController : ControllerBase
    {
        private IMatchManagementRepository  _matchmanagementrepository;
        public MatchManagementController(IMatchManagementRepository matchManagementRepository)
        {
            _matchmanagementrepository = matchManagementRepository;
        }       

        [HttpGet("GetMatches", Name = "GetMatchdetails")]
        public Task<IEnumerable<Event>> GetMatchdetails()
        {
            return _matchmanagementrepository.GetMatchDetails();
        }

        [HttpPost("PostMatches", Name = "PostMatchdetails")]
        public void PostMatchdetails(Event match)
        {
            _matchmanagementrepository.PostMatchDetails(match);
        }

        [HttpGet("GetTournament", Name = "GetTournaments")]
        public Task<Object> GeTournaments()
        {
            return _matchmanagementrepository.GetTournaments();
        }

    }
}