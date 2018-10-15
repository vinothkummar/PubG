using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Repository.Interface
{
    public interface ITeamRepository
    {
        void InsertTeam(Team team);

        Task<IEnumerable<Team>> GetTeam();

        Task<TeamLineUp> GetTeamLine(int teamId);

        Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2);

        Task<Object> GetAllTeamStats();

        Task<Object> GetTeamStats(int matchId);
        
        Task<IEnumerable<TeamRankingView>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId);

        Task<IEnumerable<TeamRankingView>> GetTeamProfileMatchUp(string teamId1, string teamId2);

        Task<IEnumerable<TeamParticipants>> GetAllTeam();

        Task<TeamLanding> GetTeamLanding(int matchId);
        void PostTeam(Team NewTeam);
    }
}
