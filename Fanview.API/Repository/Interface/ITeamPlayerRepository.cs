using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamPlayerRepository
    {        
        Task<IEnumerable<TeamPlayer>> GetPlayerMatchup(string playerId1, string playerId2);
        Task<TeamPlayer> GetPlayerProfile(string playerId1);
        void InsertTeamPlayer(TeamPlayer teamPlayer);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId1, string matchId2, string matchId3, string matchId4);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers();
        Task<TeamLineUp> GetTeamandPlayers();
    }
}
