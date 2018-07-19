﻿using System;
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
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId1, string matchId2, string matchId3, string matchId4);
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers();
        Task<TeamLineUp> GetTeamandPlayers();
        Task<IEnumerable<CreatePlayer>> GetPlayersCreated(string matchId);
        Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersTournament(int playerId);
        Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersTournament(int playerId, string matchId);
        Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersStatsMatchUp(int playerId1, int playerId2, string matchId);
        Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfilesMatchUP(int playerId1, int playerId2);
    }
}
