using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private ITeamPlayerRepository _teamPlayerRepository;

        public PlayerController(ITeamPlayerRepository teamPlayerRepository)
        {
            _teamPlayerRepository = teamPlayerRepository;            
        }


        ////// POST: api/Player
        ////[HttpPost("Create")]
        ////public void Post([FromBody] TeamPlayer value)
        ////{
        ////    _teamPlayerRepository.InsertTeamPlayer(value);
        ////}

        ///// <summary>
        ///// Returns Player Profile for the given playerId1     
        ///// </summary>
        ///// <remarks>
        ///// Sample request: Profile/{playerId1}
        ///// </remarks>
        ///// <param name='playerId1'>5b3e63846c0810ce58c29997</param>      
        //[HttpGet("Profile/{playerId1}", Name = "GetPlayerProfile")]
        //public Task<TeamPlayer> GetPlayerProfile(string playerId1)
        //{
        //    return _teamPlayerRepository.GetPlayerProfile(playerId1);
        //}

        /// <summary>
        /// Returns Tournament Total Match Stats   
        /// </summary> 
        [HttpGet("Stats/Overall", Name = "GetPlayerProfileTournament")]
        public Task<object>GetPlayerProfileTournament()
        {   
            return _teamPlayerRepository.GetTeamPlayersTournament();          
        }
   
        [HttpGet("Profile/{playerId1}/{matchId}", Name = "GetPlayerProfileTournamentByMatchId")]
        public Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfileTournamentByMatchId(int playerId1, int matchId)
        {
            return _teamPlayerRepository.GetTeamPlayersTournament(playerId1, matchId);
        }

        [HttpGet("Profile/MatchUp/{playerId1}/{playerId2}", Name = "GetPlayerProfilesMatchUP")]
        public Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfilesMatchUP(int playerId1, int playerId2)
        {
            return _teamPlayerRepository.GetPlayerProfilesMatchUP(playerId1, playerId2);
        }

        [HttpGet("Profile/MatchUp/{playerId1}/{playerId2}/{matchId}", Name = "GetPlayerProfileMatchUPByMatchId")]
        public Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfileMatchUPByMatchId(int playerId1, int playerId2, int matchId)
        {
            return _teamPlayerRepository.GetTeamPlayersStatsMatchUp(playerId1, playerId2, matchId);
        }

        ////GET:api/TeamPlayer
        ///// <summary>
        ///// Returns All Team Players    
        ///// </summary>
        ///// <remarks>
        ///// Sample request: GetAllPlayers
        ///// </remarks>
        [HttpGet("All", Name = "GetAllPlayer")]
        public async Task<IEnumerable<PlayerAll>> GetAllPlayer()
        {
            try
            {
                var result = new List<PlayerAll>();
                foreach (var item in _teamPlayerRepository.GetTeamPlayers().Result)
                {
                    result.Add(
                    new PlayerAll()
                    {
                        TeamId = item.TeamIdShort,
                        PlayerId = item.PlayerId,
                        PlayerName = item.PlayerName,
                        FullName = item.FullName,
                        Country = item.Country
                    });


                }

               
                return await Task.FromResult(result);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
        }

        //////GET:api/TeamLineUp
        /////// <summary>
        /////// Returns All players in a specific team 
        /////// </summary>
        /////// <remarks>
        /////// Sample request: GetAllTeamPlayers
        /////// </remarks>
        //[HttpGet("TeamPlayers", Name = "GetAllPlayerinTeam")]
        //public Task<TeamLineUp> GetAllTeamPlayers()
        //{
        //    return _teamPlayerRepository.GetTeamandPlayers();
        //}
    }
}
