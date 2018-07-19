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
        private IGenericRepository<TeamRanking> _teamRankings;
        private IGenericRepository<Event> _tournament;
        private LiveGraphichsDummyData _data;
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IGenericRepository<Team> team,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,                              
                              IGenericRepository<TeamPlayer> teamPlayers,
                              IGenericRepository<TeamRanking> teamRankings,
                              IGenericRepository<Event> tournament,                              
                              ILogger<TeamRepository> logger)
        {
            _team = team;
            _matchPlayerStats = matchPlayerStats;
            _teamPlayers = teamPlayers;
            _teamRankings = teamRankings;
            _tournament = tournament;

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
      

        public async Task<IEnumerable<TeamRanking>> GetTeamProfile(string teamId1)
        {

            var teamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = teamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => cn.TeamId == teamId1 )).Result.ToListAsync();

            var teamRanks = teamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
            new
            {
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName),
                TotalScore = s.Sum(a => a.TotalPoints)
            }).OrderByDescending(o => o.TotalScore).ToListAsync().Result.ToList()
            .Select((item, index) => new { TeamId = item.TeamId, TotalScore = item.TotalScore, Rank = index });

            var teamPosition = teamRanks.Where(cn => cn.TeamId == teamId1).Select(s => s.Rank).FirstOrDefault() + 1;

            var teamStandings = teamScrore.Result.GroupBy(g =>  g.TeamId)
                 .Select(s => new TeamRanking()
               {           
                TeamRank = teamPosition.ToString(),
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),                
                Kill  = s.Sum(a => a.Kill),
                Damage = s.Sum(a => a.Damage),
                TotalPoints = s.Sum( a => a.TotalPoints)                             
            });

            return await Task.FromResult(teamStandings);
        }

        public async void InsertTeam(Team team)
        {
            Func<Task> persistDataToMongo = async () => _team.Insert(team, "Team");

            await Task.Run(persistDataToMongo);
        }

         public async Task<IEnumerable<TeamRanking>> GetTeamProfileByMatchId(string teamId1, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = teamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => cn.TeamId == teamId1 && cn.MatchId == tournamentMatchId)).Result.ToListAsync();

            var teamRanks = teamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
            new
            {
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName),
                TotalScore = s.Sum(a => a.TotalPoints)
            }).OrderByDescending(o => o.TotalScore).ToListAsync().Result.ToList()
            .Select((item, index) => new { TeamId = item.TeamId, TotalScore = item.TotalScore, Rank = index });

            var teamPosition = teamRanks.Where(cn => cn.TeamId == teamId1).Select(s => s.Rank).FirstOrDefault() + 1;

            var teamStandings = teamScrore.Result.GroupBy(g => g.TeamId)
                 .Select(s => new TeamRanking()
                 {   MatchId = s.Select(a =>a.MatchId).ElementAtOrDefault(0),
                     TeamRank = teamPosition.ToString(),
                     TeamId = s.Key,
                     TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),
                     Kill = s.Sum(a => a.Kill),
                     Damage = s.Sum(a => a.Damage),
                     TotalPoints = s.Sum(a => a.TotalPoints)
                 });

            return await Task.FromResult(teamStandings);
        }

        public async Task<IEnumerable<TeamRanking>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = teamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => (cn.TeamId == teamId1 || cn.TeamId == teamId2 ) && (cn.MatchId == tournamentMatchId))).Result.ToListAsync();

            var teamRanks = teamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
            new
            {
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName),
                TotalScore = s.Sum(a => a.TotalPoints)
            }).OrderByDescending(o => o.TotalScore).ToListAsync().Result.ToList()
            .Select((item, index) => new { TeamId = item.TeamId, TotalScore = item.TotalScore, Rank = index });

            var teamPosition = teamRanks.Where(cn => cn.TeamId == teamId1 || cn.TeamId == teamId2).Select(s => new { TeamId = s.TeamId, Rank = s.Rank }).ToList();

            var i = 0;

            var teamStandings = teamScrore.Result.GroupBy(g => g.TeamId)
                 .Select(s => new TeamRanking()
                 {
                     MatchId = s.Select(a => a.MatchId).ElementAtOrDefault(0),
                     TeamRank = (teamPosition.Select(a => a.Rank).ElementAtOrDefault(i++) + 1).ToString(),
                     TeamId = s.Key,
                     TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),
                     Kill = s.Sum(a => a.Kill),
                     Damage = s.Sum(a => a.Damage),
                     TotalPoints = s.Sum(a => a.TotalPoints)
                 });

            return await Task.FromResult(teamStandings);
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


        public async Task<IEnumerable<TeamRanking>> GetTeamProfileMatchUp(string teamId1, string teamId2)
        {
            var teamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = teamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => cn.TeamId == teamId1 || cn.TeamId == teamId2)).Result.ToListAsync();

            var teamRanks = teamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
            new
            {
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName),
                TotalScore = s.Sum(a => a.TotalPoints)
            }).OrderByDescending(o => o.TotalScore).ToListAsync().Result.ToList()
            .Select((item, index) => new { TeamId = item.TeamId, TotalScore = item.TotalScore, Rank = index });

            var teamPosition = teamRanks.Where(cn => cn.TeamId == teamId1 || cn.TeamId == teamId2).Select(s => new { TeamId = s.TeamId, Rank = s.Rank }).ToList();

            var i = 0;

            var teamStandings = teamScrore.Result.GroupBy(g => g.TeamId)
                 .Select(s => new TeamRanking()
                 {
                     TeamRank = (teamPosition.Select(a => a.Rank).ElementAtOrDefault(i++) + 1).ToString(),
                     TeamId = s.Key,
                     TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),
                     Kill = s.Sum(a => a.Kill),
                     Damage = s.Sum(a => a.Damage),
                     TotalPoints = s.Sum(a => a.TotalPoints)
                 });

            return await Task.FromResult(teamStandings);
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
