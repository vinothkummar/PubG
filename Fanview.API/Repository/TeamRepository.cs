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
        private IGenericRepository<PlayerPoition> _teamPlayersPosition;
        private IGenericRepository<TeamRanking> _teamRankings;
        private IGenericRepository<Event> _tournament;
        private LiveGraphichsDummyData _data;
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IGenericRepository<Team> team,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,                              
                              IGenericRepository<TeamPlayer> teamPlayers,
                              IGenericRepository<TeamRanking> teamRankings,
                              IGenericRepository<Event> tournament,
                              IGenericRepository<PlayerPoition> teamPlayersPosition,
                              ILogger<TeamRepository> logger)
        {
            _team = team;
            _matchPlayerStats = matchPlayerStats;
            _teamPlayers = teamPlayers;
            _teamRankings = teamRankings;
            _tournament = tournament;
            _teamPlayersPosition = teamPlayersPosition;

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
            var tournaments = _tournament.GetMongoDbCollection("TournamentM;atchId");

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

        public Task<TeamLineUp> GetTeamLine(int teamId)
        {
            var teams = _team.GetAll("Team").Result;

            var teamPlayers = _teamPlayers.GetAll("TeamPlayers").Result;

            var query = teams.GroupJoin(teamPlayers, tp => tp.TeamId, t => t.TeamIdShort, (t, tp) => new
            {

                teamid = t.TeamId,

                TeamName = t.Name,

                TeamPlayers = tp.Select(s => new { s.TeamIdShort, s.PlayerName, s.PlayerId }).Distinct()

            }).Where(t => t.teamid == teamId);

            var teamlineup = new TeamLineUp();

            var teamplayer = new List<TeamLineUpPlayers>();

            foreach (var obj in query)
            {
                teamlineup.TeamId = obj.teamid.ToString();

                teamlineup.TeamName = obj.TeamName;

                foreach (var players in obj.TeamPlayers)
                {
                    teamplayer.Add(new TeamLineUpPlayers() { PlayerName = players.PlayerName, TeamId = players.TeamIdShort, PlayerId = players.PlayerId });
                }

                teamlineup.TeamPlayer = teamplayer;



            }

            return Task.FromResult(teamlineup);
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
        public Task<IEnumerable<TeamRoute>> GetTeamRoute(int matchId)
        {
            var teams = _team.GetMongoDbCollection("Team").AsQueryable();

            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = teamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync().Result;

            var logPlayersPosition = _teamPlayersPosition.GetMongoDbCollection("PlayerPosition");

            var matchPlayerPosition = logPlayersPosition.FindAsync(Builders<PlayerPoition>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result
                                        .ToListAsync().Result.OrderByDescending(o => o.EventTimeStamp);

            var playerLocation = matchPlayerPosition.Join(teams, mpp => mpp.TeamId , t=> t.TeamId,(mpp,t) => new {mpp, t})                           
                                                    .Select(s => new
                                                    {
                                                        TeamName = s.t.Name,
                                                        PlayerName = s.mpp.Name,
                                                        Health = s.mpp.Health,
                                                        s.mpp.NumAlivePlayers,
                                                        s.mpp.Location,
                                                        s.mpp.EventTimeStamp,
                                                        PubTeamId = s.mpp.TeamId,
                                                        FanviewTeamId = s.t.TeamId.ToString(),
                                                        Timestamp = s.mpp.EventTimeStamp
                                                    });


            var teamRanks = teamScrore.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
            new
            {
                TeamId = s.Key,
                TeamName = s.Select(a => a.TeamName),
                TotalScore = s.Sum(a => a.TotalPoints)
            }).OrderByDescending(o => o.TotalScore)
            .Select((item, index) => new { TeamId = item.TeamId, TotalScore = item.TotalScore, Rank = index }).Take(3);

            var longestSurvivingTeamPlayers = playerLocation.Join(teamRanks, pl => new { TeamId = pl.FanviewTeamId }, tr => new { TeamId = tr.TeamId },
                                                            (pl, tr) => new { pl, tr }).Select(s =>
                                                             new TeamRoute()
                                                             {
                                                                 MatchId = 7,
                                                                 Routs = new Route()
                                                                 {
                                                                     TeamID = s.pl.FanviewTeamId,
                                                                     TeamName = s.pl.TeamName,
                                                                     TeamRank = s.tr.Rank,
                                                                     TeamRoute = s.pl.Location,
                                                                     PlayerName = s.pl.PlayerName
                                                                 }
                                                             });
                                                             

            return  Task.FromResult(longestSurvivingTeamPlayers);
        }

        public Task<TeamLanding> GetTeamLanding()
        {
            return Task.FromResult(_data.GetTeamLanding());
        }

       
    }
}
