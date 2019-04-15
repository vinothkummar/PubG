using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.BusinessLayer.Contracts;


namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private ITeamRepository _teamRepository;
        
        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
           
        }       

        /// <summary>
        /// Returns All Teams  
        /// </summary>       
        [HttpGet("All", Name = "GetAllTeam")]
        public Task<IEnumerable<TeamParticipants>> GetAllTeam()
        {
            return _teamRepository.GetAllTeam();
        }
        [HttpGet("GetTeamDetails", Name = "GetTeamDetails")]
        public Task<IEnumerable<Team>> GetTeamDetails()
        {
            return _teamRepository.GetTeam();
        }
        [HttpPost("PostNewTeam",Name = "PostNewTeam")]
        public void PostNewTeam(Team team)
        {
            _teamRepository.PostTeam(team);
        }
        [HttpPost("UpdateManyTeams", Name = "UpdateManyTeams")]
        public void UpdateManyTeams(IEnumerable<Team> teams)
        {
            _teamRepository.UpdatemanyTeams(teams);
        }
        [HttpDelete("Deleteteam/{teamId}", Name = "Deleteteam")]
        public void DeleteTeam(string teamid)
        {
            _teamRepository.DeleteTeam(teamid);
        }

        /// <summary>
        /// Returns Team Stats for all the teams  
        /// </summary>
        /// <remarks>
        [HttpGet("Stats/Overall", Name = "GetAllTeamStats")]
        public Task<List<TeamProfile>> GetAllTeamStats()
        {
            return _teamRepository.GetAllTeamStats();
        }
        /// <summary>
        /// Returns Average Team Stats   
        /// </summary>
        /// <remarks>
        [HttpGet("Stats/Average", Name = "GetAverageTeamStats")]
        public Task<IEnumerable<TeamProfile>> GetAverageTeamStats()
        {
            return _teamRepository.GetTeamAverageStats();
        }
        /// <summary>
        /// Returns Sum of Overal Team Stats for Available Phase
        /// </summary>
        /// <remarks>
        [HttpGet("Stats/YearlyOveralTeamStats", Name = "YearlyOveralTeamStats")]
        public Task<IEnumerable<TeamProfile>> YearlyOveralTeamStats()
        {
            return _teamRepository.GetAccumulatedTeamStats();
        }
        /// <summary>
        /// Returns Sum of Average Team Stats for Available Phase
        /// </summary>
        /// <remarks>
        [HttpGet("Stats/YearlyAverageTeamStats", Name = "YearlyAverageTeamStats")]
        public Task<IEnumerable<TeamProfile>> YearlyAverageStats()
        {
            return _teamRepository.GetAccumulatedTeamAverageStats();
        }
        [HttpDelete("DeleteAllteam", Name = "DeleteAllteam")]
        public void DeleteAll()
        {
            _teamRepository.DeleteAll();
        }

        [HttpGet("Stats/{matchId}", Name = "GetTeamStats")]
        public Task<object> GetTeamStats(int matchId)
        {
            return _teamRepository.GetTeamStats(matchId);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}", Name = "GetTeamProfileMatchUp")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfileMatchUp(string teamId1, string teamId2)
        {
            return _teamRepository.GetTeamProfileMatchUp(teamId1, teamId2);
        }

        [HttpGet("Profile/MatchUp/{teamId1}/{teamId2}/{matchId}", Name = "GetTeamProfilesByTeamIdAndMatchId")]
        public Task<IEnumerable<TeamRankingView>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId)
        {
            return _teamRepository.GetTeamProfilesByTeamIdAndMatchId(teamId1, teamId2, matchId);
        }

    }
}
