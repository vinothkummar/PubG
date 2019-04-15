using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamPlayerRepository
    {        
       
        Task<TeamPlayer> GetPlayerProfile(string playerId1);
        void InsertTeamPlayer(TeamPlayer teamPlayer);        
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId1, string matchId2, string matchId3, string matchId4);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers();
        Task<IEnumerable<TeamPlayer>> GetTeamPlayersNonCached();
        Task<TeamLineUp> GetTeamandPlayers();
        Task<IEnumerable<CreatePlayer>> GetPlayersCreated(string matchId);
        Task<List<PlayerProfileTournament>> GetPlayerTournamentStats();
        Task<Object> GetPlayerTournamentStats(int matchId);
        Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersStatsMatchUp(int playerId1, int playerId2, int matchId);
        Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfilesMatchUP(int playerId1, int playerId2);
        Task<IEnumerable<PlayerKilledGraphics>> GetPlayersId(IEnumerable<LiveEventKill> liveEventKills);
        void CreateNewPlayer(TeamPlayerViewModel player);
        void Deleteplayer(string playerid);
        void Updatemanyplayers(IEnumerable<TeamPlayer> players);
        void DeleteAllTeamPlayers();
        Task<List<PlayerProfileTournament>> GetPlayerTournamentAverageStats();
        Task<IEnumerable<PlayerProfileTournament>> AccumulateOveralPlayerstate();
        Task<IEnumerable<PlayerProfileTournament>> AccumulatedAveragePlayerstate();
    }
}
