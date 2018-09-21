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

        [HttpGet("Matches", Name = "getmatchdetails")]
        public Task<IEnumerable<Event>> getmatchdetails()
        {
            return _matchmanagementrepository.GetMatchDetails();
        }



    }
}