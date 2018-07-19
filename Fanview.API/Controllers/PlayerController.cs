using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;

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


        //// POST: api/Player
        //[HttpPost("Create")]
        //public void Post([FromBody] TeamPlayer value)
        //{
        //    _teamPlayerRepository.InsertTeamPlayer(value);
        //}
        
        /// <summary>
        /// Returns Player MatchUp Response for the given playerId1 And playerId2     
        /// </summary>
        /// <remarks>
        /// Sample request: MatchUp/{playerId1}/And/{playerId2}
        /// </remarks>
        /// <param name='playerId1'>5b3e63846c0810ce58c29997</param>
        /// <param name='playerId2'>5b3e63846c0810ce58c29998</param>
        [HttpGet("MatchUp/{playerId1}/And/{playerId2}", Name = "GetPlayerMatchup")]
        public Task<IEnumerable<TeamPlayer>> GetPlayerMatchup(string playerId1, string playerId2)
        {
            return _teamPlayerRepository.GetPlayerMatchup(playerId1, playerId2);
        }

       
        /// <summary>
        /// Returns Player Profile for the given playerId1     
        /// </summary>
        /// <remarks>
        /// Sample request: Profile/{playerId1}
        /// </remarks>
        /// <param name='playerId1'>5b3e63846c0810ce58c29997</param>      
        [HttpGet("Profile/{playerId1}", Name = "GetPlayerProfile")]
        public Task<TeamPlayer> GetPlayerProfile(string playerId1)
        {
            return _teamPlayerRepository.GetPlayerProfile(playerId1);
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
