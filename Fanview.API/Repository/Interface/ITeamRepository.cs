using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamRepository
    {
        void InsertTeam(Team team);

        Task<IEnumerable<Team>> GetTeam();

        Task<IEnumerable<TeamLineUp>> GetTeamLine(string teamId);

        Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2);

        Task<TeamLineUp> GetTeamProfile(string teamId1);
        Task<IEnumerable<Team>> GetAllTeam();
        Task<IEnumerable<TeamPlayer>> GetTeamPlayers();
    }
}
