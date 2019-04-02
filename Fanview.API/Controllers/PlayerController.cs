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

        /// <summary>
        /// Returns All Team Players    
        /// </summary>
        /// <remarks>
        /// Sample request: GetAllPlayers
        /// </remarks>
        [HttpGet("All", Name = "GetAllPlayer")]
        public async Task<IEnumerable<PlayerAll>> GetAllPlayer()
        {
            try
            {
                var result = new List<PlayerAll>();
                foreach (var item in _teamPlayerRepository.GetTeamPlayersNonCached().Result)
                {
                    result.Add(
                    new PlayerAll()
                    {
                        Id = item.Id,
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

        /// <summary>
        /// Returns Tournament Total Match Stats   
        /// </summary> 
        [HttpGet("Stats/Overall", Name = "GetTournamentPlayerStats")]
        public Task<List<PlayerProfileTournament>> GetTournamentPlayerStats()
        {
            return _teamPlayerRepository.GetPlayerTournamentStats();
        }
        /// <summary>
        /// Returns Tournament Average Match Stats   
        /// </summary> 
        [HttpGet("Stats/Average", Name = "GetTournamentAveragePlayerStats")]
        public Task<List<PlayerProfileTournament>> GetTournamentAveragePlayerStats()
        {
            return _teamPlayerRepository.GetPlayerTournamentAverageStats();
        }
        /// <summary>
        /// Returns Overal Stats for a Year
        /// </summary> 
        [HttpGet("Stats/YearlyOveralStats", Name = "YearlyOveralStats")]
        public Task<IEnumerable<PlayerProfileTournament>> YearlyOveralPlayerStats()
        {
            return  _teamPlayerRepository.AccumulateOveralPlayerstate();
        }
        /// <summary>
        /// Returns Average Stats for a Year 
        /// </summary> 
        [HttpGet("Stats/YearlyAverageStats", Name = "YearlyAverageStats")]
        public Task<IEnumerable<PlayerProfileTournament>> YearlyAveragePlayerStats()
        {
            return _teamPlayerRepository.AccumulatedAveragePlayerstate();
        }

        [HttpGet("Stats/{matchId}", Name = "GetPlayerTournamentStats")]
        public Task<object> GetPlayerTournamentStats(int matchId)
        {
            return _teamPlayerRepository.GetPlayerTournamentStats(matchId);
        }

        [HttpGet("MatchUp/{playerId1}/{playerId2}", Name = "GetPlayerProfilesMatchUP")]
        public Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfilesMatchUP(int playerId1, int playerId2)
        {
            return _teamPlayerRepository.GetPlayerProfilesMatchUP(playerId1, playerId2);
        }

        [HttpGet("MatchUp/{playerId1}/{playerId2}/{matchId}", Name = "GetPlayerProfileMatchUPByMatchId")]
        public Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfileMatchUPByMatchId(int playerId1, int playerId2, int matchId)
        {
            return _teamPlayerRepository.GetTeamPlayersStatsMatchUp(playerId1, playerId2, matchId);
        }
   
        [HttpPost("CreatePlayer", Name = "PostNewPlayer")]
        public void PostNewPlayer(TeamPlayerViewModel player)
        {
            _teamPlayerRepository.CreateNewPlayer(player);
        }
        [HttpDelete("Deleteplayer/{PlayerId}", Name = "Deleteplayer")]

        public void DeletePLayer(string playerid)
        {
            _teamPlayerRepository.Deleteplayer(playerid);
        }
        
        [HttpPost("UpdateManyPlayers",Name = "UpdateManyPlayers")]
        public void UpdateManyPlayers(List<TeamPlayer> players)
        {
            _teamPlayerRepository.Updatemanyplayers(players);
        }
        [HttpDelete("DeleteAllplayers", Name = "DeleteAllplayers")]
        public void DeleteAllPlayers()
        {
            _teamPlayerRepository.DeleteAllTeamPlayers();
        }
    }
}
