using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Fanview.API.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private IGenericRepository<Team> _team;
        private IGenericRepository<MatchPlayerStats> _matchPlayerStats;
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IGenericRepository<Team> team,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,
                              ILogger<TeamRepository> logger)
        {
            _team = team;
            _matchPlayerStats = matchPlayerStats;

            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetTeam()
        { 
            var response = _team.GetAll("Team");

            return await response;
        }
     
        public async void InsertTeam(Team team)
        {
            Func<Task> persistDataToMongo = async () => _team.Insert(team, "Team");

            await Task.Run(persistDataToMongo);
        }

        async Task<IEnumerable<TeamLineUp>> ITeamRepository.GetTeamLine(string teamId)
        {
            var teamLineUp = _matchPlayerStats.GetAll("MatchPlayerStats").Result.Join(_team.GetAll("Team").Result,
                                                                                 mp => mp.TeamId, t => t.Id, (mp, t) => new { mp, t })
                                                                               .Where(cn => cn.mp.TeamId == teamId)
                                                                               .Select(s => new TeamLineUp()
                                                                               {
                                                                                   TeamId = s.t.Id,
                                                                                   TeamName = s.t.Name,
                                                                                   TeamPlayer = new List<TeamLineUpPlayers>(){
                                                                                       new TeamLineUpPlayers()
                                                                                       {
                                                                                           PlayerName = s.mp.stats.Name,
                                                                                           PubgAccountId = s.mp.stats.PlayerId,
                                                                                           Kills = s.mp.stats.Kills,
                                                                                           TimeSurvived = s.mp.stats.TimeSurvived
                                                                                       }
                                                                                   }
                                                                               });
            return await Task.FromResult(teamLineUp);
        }
    }
}
