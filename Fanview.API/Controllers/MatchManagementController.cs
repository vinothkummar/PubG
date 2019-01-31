using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;



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

        [HttpGet("GetPlayerDeskPositions", Name = "GetPlayerDeskPositions")]
        public Task<IEnumerable<DeskSeatings>> GetPlayerDeskPositions()
        {
            return _matchmanagementrepository.GetPlayerDeskPositions();
        }

        [HttpPost("CreatePlayerDeskPosition", Name = "CreatePlayerDeskPosition")]
        public void CreatePlayerDeskPosition([FromBody]IEnumerable<DeskSeatings> seatingPosition)
        {
            _matchmanagementrepository.CreatePlayerDeskPosition(seatingPosition);
        }

        [HttpPut("EditPlayerDeskPosition", Name = "EditPlayerDeskPosition")]
        public void EditPlayerDeskPosition([FromBody]IEnumerable<DeskSeatings> seatingPosition)
        {
            _matchmanagementrepository.EditPlayerDeskPosition(seatingPosition);
        }

        [HttpGet("GetTournaments", Name = "GetTournaments")]
        public Task<Object> GeTournaments()
        {
            return _matchmanagementrepository.GetTournaments();
        }

        [HttpGet("GetTournamentMatches", Name = "GetTournamentsMatches")]
        public Task<Object> GeTournamentsMatches(string tournamentName)
        {
            return _matchmanagementrepository.GetTournamentsMatches(tournamentName);
        }

        [HttpGet("GetLiveLatestMatchId", Name = "GetLiveLatestMatchId")]
        public Task<Object> GetLiveLatestMatch(string tournamentName)
        {
            return _matchmanagementrepository.GetLiveLatestMatch(tournamentName);
        }

        [HttpDelete("DeleteMatch/{matchId}", Name = "DeleteMatch")]
        public dynamic DeleteMatch(string matchId)
        {
            return _matchmanagementrepository.DeleteDocumentCollections(matchId);
        }
    }
}