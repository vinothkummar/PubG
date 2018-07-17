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
using Fanview.API.Model.LiveModels;
using Fanview.API.GraphicsDummyData;

namespace Fanview.API.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private IGenericRepository<Team> _team;
        private IGenericRepository<MatchPlayerStats> _matchPlayerStats;
        private IGenericRepository<TeamPlayer> _teamPlayers;
        private LiveGraphichsDummyData _data;
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IGenericRepository<Team> team,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,                              
                              IGenericRepository<TeamPlayer> teamPlayers,
                              ILogger<TeamRepository> logger)
        {
            _team = team;
            _matchPlayerStats = matchPlayerStats;
            _teamPlayers = teamPlayers;

            _data = new LiveGraphichsDummyData();

            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetTeam()
        { 
            var response = _team.GetAll("Team");

            return await response;
        }

        public async Task<IEnumerable<TeamLineUp>> GetTeamMatchup(string teamId1, string teamId2)
        {
            var teamCollection = _team.GetMongoDbCollection("Team");

            var teamPlayerCollection = _teamPlayers.GetMongoDbCollection("TeamPlayers");

            var teams = await teamCollection.FindAsync(Builders<Team>.Filter.Where(cn => cn.Id == teamId1 || cn.Id == teamId2)).Result.ToListAsync();

            var teamPlayers = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.TeamId == teamId1 || cn.TeamId == teamId2)).Result.ToListAsync();

            var teamLineups = new List<TeamLineUp>();

            var query = teams.GroupJoin(teamPlayers, tp => tp.Id, t => t.TeamId, (t, tp) => new
            {
                TeamId = t.Id,
                TeamName = t.Name,
                TeamPlayers = tp.Select(s => s.PlayerName)
            });

            var teamLineupMatch = new List<TeamLineUp>();

            foreach (var obj in query)
            {
                var teamLineup = new TeamLineUp();

                teamLineup.TeamId = obj.TeamId;
                teamLineup.TeamName = obj.TeamName;

                var tmPlayers = new List<TeamLineUpPlayers>();

                foreach (var players in obj.TeamPlayers)
                {
                    tmPlayers.Add(new TeamLineUpPlayers() { PlayerName = players });
                }

                teamLineup.TeamPlayer = tmPlayers;

                teamLineupMatch.Add(teamLineup);
            }



            return await Task.FromResult(teamLineupMatch);
            
        }
        public async Task<IEnumerable<Team>> GetAllTeam()
        {
            var result= await _team.GetAll("Team");            
            return result;
        }
      

        public async Task<TeamLineUp> GetTeamProfile(string teamId1)
        {
            var teamCollection = _team.GetMongoDbCollection("Team");

            var teamPlayerCollection = _teamPlayers.GetMongoDbCollection("TeamPlayers");

            var teams = await teamCollection.FindAsync(Builders<Team>.Filter.Where(cn => cn.Id == teamId1)).Result.ToListAsync();

            var teamPlayers = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.TeamId == teamId1)).Result.ToListAsync();

            var teamLineups = new List<TeamLineUp>();

            var query = teams.GroupJoin(teamPlayers, tp => tp.Id, t => t.TeamId, (t, tp) => new
            {
                TeamId = t.Id,
                TeamName = t.Name,
                TeamPlayers = tp.Select(s => s.PlayerName)
            });

            var teamLineupMatch = new TeamLineUp();

            foreach (var obj in query)
            {
                teamLineupMatch.TeamId = obj.TeamId;
                teamLineupMatch.TeamName = obj.TeamName;

                var tmPlayers = new List<TeamLineUpPlayers>();

                foreach (var players in obj.TeamPlayers)
                {
                    tmPlayers.Add(new TeamLineUpPlayers() { PlayerName = players });
                }

                teamLineupMatch.TeamPlayer = tmPlayers;
            }



            return await Task.FromResult(teamLineupMatch);
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

        public Task<TeamRoute> GetTeamRoute()
        {
            return Task.FromResult(_data.GetTeamRoute());
        }

        public Task<TeamLanding> GetTeamLanding()
        {
            return Task.FromResult(_data.GetTeamLanding());
        }
    }
}
