using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamRepository
    {
        void InsertTeam(Team team);

        Task<IEnumerable<Team>> GetTeam();

        Task<TeamLineUp> GetTeamLine(int teamId);

        Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2);

        Task<IEnumerable<TeamRanking>> GetTeamProfile(string teamId1);

        Task<IEnumerable<TeamRanking>> GetTeamProfileByMatchId(string teamId1, int matchId);


        Task<IEnumerable<TeamRanking>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId);

        Task<IEnumerable<TeamRanking>> GetTeamProfileMatchUp(string teamId1, string teamId2);
        Task<IEnumerable<Team>> GetAllTeam();

        Task<IEnumerable<TeamRoute>> GetTeamRoute(int matchId);

        Task<TeamLanding> GetTeamLanding();


    }
}
