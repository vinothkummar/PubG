﻿using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private IGenericRepository<Team> _team;
        private IGenericRepository<MatchPlayerStats> _matchPlayerStats;
        private IGenericRepository<TeamPlayer> _teamPlayers;
        private IGenericRepository<PlayerPoition> _teamPlayersPosition;
        private IMatchRepository _matchRepository;
        private IGenericRepository<TeamRanking> _teamRankings;
        private IGenericRepository<ParachuteLanding> _parachuteLanding;
        private IGenericRepository<Event> _tournament;
        private IGenericRepository<TeamProfile> _TeamStats;

        private ILogger<TeamRepository> _logger;
        private ICacheService _cacheService;

        public TeamRepository(IGenericRepository<Team> team,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,                              
                              IGenericRepository<TeamPlayer> teamPlayers,
                              IGenericRepository<TeamRanking> teamRankings,
                              IGenericRepository<Event> tournament,
                              IGenericRepository<ParachuteLanding> parachuteLanding,
                              IGenericRepository<PlayerPoition> teamPlayersPosition,   
                              IMatchRepository matchRepository,
                              ICacheService cacheService,
                              ILogger<TeamRepository> logger,
                              IGenericRepository<TeamProfile> TeamStats)
        {
            _team = team;
            _matchPlayerStats = matchPlayerStats;
            _teamPlayers = teamPlayers;
            _teamRankings = teamRankings;
            _tournament = tournament;
            _parachuteLanding = parachuteLanding;
            _teamPlayersPosition = teamPlayersPosition;
            _matchRepository = matchRepository;
            _logger = logger;
            _cacheService = cacheService;
            _TeamStats = TeamStats;
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

        public async Task<IEnumerable<TeamParticipants>> GetAllTeam()
        {
            var teams =  _team.GetAll("Team").Result;

            var teamPlayers = _teamPlayers.GetAll("TeamPlayers").Result;

            var query = teams.GroupJoin(teamPlayers, tp => tp.TeamId, t => t.TeamIdShort, (t, tp) => new
            {
                teamid = t.TeamId,

                TeamName = t.Name,

                ShortName = t.ShortName,

                TeamPlayers = tp.Select(s => new { s.TeamIdShort, s.PlayerName, s.PlayerId }).Distinct()

            });

            var teamLineUpCollections = new List<TeamParticipants>();

            foreach (var obj in query)
            {

                var teamlineup = new TeamParticipants();

                teamlineup.TeamId = obj.teamid;

                teamlineup.TeamName = obj.TeamName;

                teamlineup.ShortName = obj.ShortName;

                var teamplayer = new List<Participants>();

                foreach (var players in obj.TeamPlayers)
                {
                    teamplayer.Add(new Participants(){PlayerName = players.PlayerName, PlayerId = players.PlayerId });
                }

                teamlineup.TeamPlayer = teamplayer;

                teamLineUpCollections.Add(teamlineup);

            }
            return await Task.FromResult(teamLineUpCollections);
        }
         
        public async Task<List<TeamProfile>> GetAllTeamStats()
        {

            var teamCollection = _team.GetMongoDbCollection("Team");

            var teams = await teamCollection.FindAsync(Builders<Team>.Filter.Empty).Result.ToListAsync();

            var matchstats = _matchPlayerStats.GetAll("MatchPlayerStats").Result;

            var teamStats = matchstats.Join(teams, ms => ms.TeamId, t => t.Id, (ms, t) => new { ms, t })
                                          .Select(s => new
                                          {
                                              MatchId = s.ms.MatchId,
                                              TeamId = s.t.TeamId,
                                              Name = s.t.Name,
                                              Region = s.t.Region,
                                              ShortName = s.t.ShortName,
                                              Stats = new
                                              {
                                                  Knocs = s.ms.stats.DBNOs,
                                                  Assists = s.ms.stats.Assists,
                                                  Boosts = s.ms.stats.Boosts,
                                                  Damage = s.ms.stats.DamageDealt,
                                                  HeadShort = s.ms.stats.HeadshotKills,
                                                  Heals = s.ms.stats.Heals,
                                                  Kills = s.ms.stats.Kills,
                                                  TimeSurvived = s.ms.stats.TimeSurvived,
                                                  Revives = s.ms.stats.Revives,
                                                  RideDistance = s.ms.stats.RideDistance,
                                                  SwimDistance = s.ms.stats.SwimDistance,
                                                  WalkDistance = s.ms.stats.WalkDistance
                                              }
                                          });
            var matchcount = teamStats.GroupBy(x => x.TeamId).Select(group => new { teamid = group.Key, NumMatches = group.Select(x => x.MatchId).Distinct().Count()});
            var teamStatsGrouped = teamStats.GroupBy(g => g.TeamId).Select(s => new 
            {
                TeamId = s.Key,
                Name = s.Select(a => a.Name).ElementAtOrDefault(0),
                Region = s.Select(a => a.Region).ElementAtOrDefault(0),
                ShortName = s.Select(a => a.ShortName).ElementAtOrDefault(0),
                NumMatches=s.Select(a=>a.MatchId).Distinct().Count(),
                stats = new Stats()
                {
                    Knocks = s.Sum(a => a.Stats.Knocs),
                    Assists = s.Sum(a => a.Stats.Assists),
                    Boosts = s.Sum(a => a.Stats.Boosts),
                    damage = Math.Round(s.Sum(a => a.Stats.Damage), 2, MidpointRounding.AwayFromZero),
                    headShot = s.Sum(a => a.Stats.HeadShort),
                    Heals = s.Sum(a => a.Stats.Heals),
                    Kills = s.Sum(a => a.Stats.Kills),
                    TimeSurvived = s.Sum(a => a.Stats.TimeSurvived),
                    Revives = s.Sum(a => a.Stats.Revives),
                    RideDistance = Math.Round(s.Sum(a => a.Stats.RideDistance), 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round(s.Sum(a => a.Stats.SwimDistance), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round(s.Sum(a => a.Stats.WalkDistance), 2, MidpointRounding.AwayFromZero)
                }
            }).OrderBy(o => o.TeamId);
            var TeamStats = teamStatsGrouped.Select(a => new TeamProfile()
            {
                TeamId=a.TeamId,
                NumMatches=a.NumMatches,
                Region=a.Region,
                ShortName=a.ShortName,
                Name=a.Name,
                stats=a.stats
            }).ToList();
         
            return TeamStats ;

            
        }
        public async Task<IEnumerable<TeamProfile>> GetTeamAverageStats()
        {
            var teamCollection = _team.GetMongoDbCollection("Team");

            var teams = await teamCollection.FindAsync(Builders<Team>.Filter.Empty).Result.ToListAsync();

            var matchstats = _matchPlayerStats.GetAll("MatchPlayerStats").Result;

            var TeamStats = matchstats.Join(teams, ms => ms.TeamId, t => t.Id, (ms, t) => new { ms, t })
                                          .Select(s => new
                                          {
                                              MatchId = s.ms.MatchId,
                                              TeamId = s.t.TeamId,
                                              Name = s.t.Name,
                                              Region = s.t.Region,
                                              ShortName = s.t.ShortName,
                                              Stats = new
                                              {
                                                  Knocs = s.ms.stats.DBNOs,
                                                  Assists = s.ms.stats.Assists,
                                                  Boosts = s.ms.stats.Boosts,
                                                  Damage = s.ms.stats.DamageDealt,
                                                  HeadShort = s.ms.stats.HeadshotKills,
                                                  Heals = s.ms.stats.Heals,
                                                  Kills = s.ms.stats.Kills,
                                                  TimeSurvived = s.ms.stats.TimeSurvived,
                                                  Revives = s.ms.stats.Revives,
                                                  RideDistance = s.ms.stats.RideDistance,
                                                  SwimDistance = s.ms.stats.SwimDistance,
                                                  WalkDistance = s.ms.stats.WalkDistance
                                              }
                                          });

            var TeamStatsGrouped = TeamStats.GroupBy(g => g.TeamId).Select(s => new
            {
                TeamId = s.Key,
                Name = s.Select(a => a.Name).ElementAtOrDefault(0),
                Region = s.Select(a => a.Region).ElementAtOrDefault(0),
                ShortName = s.Select(a => a.ShortName).ElementAtOrDefault(0),
                NumMatches = s.Select(a => a.MatchId).Distinct().Count(),

                stats = new Stats()
                {
                    Knocks = Math.Round((double)(s.Sum(a => a.Stats.Knocs)/(double)s.Select(a=>a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    Assists = Math.Round((double)(s.Sum(a => a.Stats.Assists) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    Boosts =Math.Round((double)s.Sum(a => a.Stats.Boosts) /(double) s.Select(a => a.MatchId).Distinct().Count(),2,MidpointRounding.AwayFromZero),
                    damage = Math.Round((double)(s.Sum(a => a.Stats.Damage) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    headShot = Math.Round((double)(s.Sum(a => a.Stats.HeadShort) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    Heals = Math.Round((double)(s.Sum(a => a.Stats.Heals) /(double)s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    Kills = Math.Round((double)(s.Sum(a => a.Stats.Kills) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    TimeSurvived = Math.Round((double)(s.Sum(a => a.Stats.TimeSurvived) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    Revives = Math.Round((double)(s.Sum(a => a.Stats.Revives) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    RideDistance = Math.Round((double)(s.Sum(a => a.Stats.RideDistance) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round((double)(s.Sum(a => a.Stats.SwimDistance) / (double)s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round((double)(s.Sum(a => a.Stats.WalkDistance) /(double) s.Select(a => a.MatchId).Distinct().Count()), 2, MidpointRounding.AwayFromZero)
                }
            }).OrderBy(o => o.TeamId);
            var averageStats = TeamStatsGrouped.Select(a => new TeamProfile()
            {
                TeamId = a.TeamId,
                NumMatches = a.NumMatches,
                Name = a.Name,
                ShortName = a.ShortName,
                Region = a.Region,
                stats = a.stats

            }).ToList();
            return averageStats;


        }

        public async void InsertTeam(Team team)
        {
            Func<Task> persistDataToMongo = async () => _team.Insert(team, "Team");

            await Task.Run(persistDataToMongo);
        }

         public async Task<Object> GetTeamStats(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamCollection = _team.GetMongoDbCollection("Team");

            var teams = await teamCollection.FindAsync(Builders<Team>.Filter.Empty).Result.ToListAsync();

            var matchstats = _matchPlayerStats.GetMongoDbCollection("MatchPlayerStats");

            var matchstat = await matchstats.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync();

            var TeamStats = matchstat.Join(teams, ms => ms.TeamId, t => t.Id, (ms, t) => new { ms, t })
                                          .Select(s => new
                                          {
                                              MatchId = s.ms.MatchId,
                                              TeamId = s.t.TeamId,
                                              Name = s.t.Name,
                                              Region = s.t.Region,
                                              ShortName = s.t.ShortName,
                                              Stats = new
                                              {
                                                  Knocs = s.ms.stats.DBNOs,
                                                  Assists = s.ms.stats.Assists,
                                                  Boosts = s.ms.stats.Boosts,
                                                  Damage = s.ms.stats.DamageDealt,
                                                  HeadShort = s.ms.stats.HeadshotKills,
                                                  Heals = s.ms.stats.Heals,
                                                  Kills = s.ms.stats.Kills,
                                                  TimeSurvived = s.ms.stats.TimeSurvived,
                                                  Revives = s.ms.stats.Revives,
                                                  RideDistance = s.ms.stats.RideDistance,
                                                  SwimDistance = s.ms.stats.SwimDistance,
                                                  WalkDistance = s.ms.stats.WalkDistance
                                              }
                                          });

            var TeamStatsGrouped = TeamStats.GroupBy(g => g.TeamId).Select(s => new
            {
                MatchId = matchId,
                TeamId = s.Key,
                Name = s.Select(a => a.Name).ElementAtOrDefault(0),
                Region = s.Select(a => a.Region).ElementAtOrDefault(0),
                ShortName = s.Select(a => a.ShortName).ElementAtOrDefault(0),
                NumMatches = s.Select(a => a.MatchId).Distinct().Count(),
                stats = new Stats()
                {
                    Knocks = s.Sum(a => a.Stats.Knocs),
                    Assists = s.Sum(a => a.Stats.Assists),
                    Boosts = s.Sum(a => a.Stats.Boosts),
                    damage = Math.Round(s.Sum(a => a.Stats.Damage), 2, MidpointRounding.AwayFromZero),
                    headShot = s.Sum(a => a.Stats.HeadShort),
                    Heals = s.Sum(a => a.Stats.Heals),
                    Kills = s.Sum(a => a.Stats.Kills),
                    TimeSurvived = s.Sum(a => a.Stats.TimeSurvived),
                    Revives = s.Sum(a => a.Stats.Revives),
                    RideDistance = Math.Round(s.Sum(a => a.Stats.RideDistance), 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round(s.Sum(a => a.Stats.SwimDistance), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round(s.Sum(a => a.Stats.WalkDistance), 2, MidpointRounding.AwayFromZero)
                }
            }).OrderBy(o => o.TeamId);

            return TeamStatsGrouped;
        }

        public async Task<IEnumerable<TeamRankingView>> GetTeamProfilesByTeamIdAndMatchId(string teamId1, string teamId2, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var TeamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");

            var teamScrore = TeamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => (cn.TeamId == teamId1 || cn.TeamId == teamId2 ) && (cn.MatchId == tournamentMatchId))).Result.ToListAsync();

            var teamRanks = TeamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
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
                 .Select(s => new TeamRankingView()
                 {
                     MatchId= tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.Id == s.FirstOrDefault().MatchId)).Result.FirstOrDefaultAsync().Result.MatchId,
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
        
        public async Task<IEnumerable<TeamRankingView>> GetTeamProfileMatchUp(string teamId1, string teamId2)
        {
            var TeamStatsRanking = _teamRankings.GetMongoDbCollection("TeamRanking");
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var teamScrore = TeamStatsRanking.FindAsync(Builders<TeamRanking>.Filter.Where(cn => cn.TeamId == teamId1 || cn.TeamId == teamId2)).Result.ToListAsync();

            var teamRanks = TeamStatsRanking.AsQueryable().GroupBy(g => g.TeamId).Select(s =>
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
                 .Select(s => new TeamRankingView()
                 {
                     TeamRank = (teamPosition.Select(a => a.Rank).ElementAtOrDefault(i++) + 1).ToString(),
                     TeamId = s.Key,
                     TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),
                     Kill = s.Sum(a => a.Kill),
                     Damage = s.Sum(a => a.Damage),
                     TotalPoints = s.Sum(a => a.TotalPoints),
                     MatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.Id == s.FirstOrDefault().MatchId)).Result.FirstOrDefaultAsync().Result.MatchId
                 });

            return await Task.FromResult(teamStandings);
        }

        public async Task<TeamLanding> GetTeamLanding(int matchId)
        {
            var teams = _team.GetMongoDbCollection("Team").AsQueryable();

            var teamPlayersCollection = _teamPlayers.GetMongoDbCollection("TeamPlayers");

            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var parachuteLandingCollection = _parachuteLanding.GetMongoDbCollection("ParachuteLanding");
            var parachuteLandingsQuery = await parachuteLandingCollection.FindAsync(Builders<ParachuteLanding>.Filter.Where(c => c.MatchId == tournamentMatchId  && c.Distance != 0));
            var parachuteLandings = await parachuteLandingsQuery.ToListAsync();

            var response = new TeamLanding();

            var landing = new List<Landing>();

            response.MatchdId = tournamentMatchId;

            response.MapName = _matchRepository.GetMapName(tournamentMatchId).Result;



            foreach (var q in teams.OrderBy(o=>o.Name))
            {
                var playersQuery = await teamPlayersCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(tp => tp.TeamId == q.Id));
                var players = await playersQuery.ToListAsync();
                var playerNames = players.Select(p => p.PlayerName);
                var filteredLandings = parachuteLandings.Where(vl => playerNames.Contains(vl.Character.Name));

                if (filteredLandings.Any())
                {
                    landing.Add(new Landing()
                    {
                        TeamID = q.TeamId,
                        TeamName = q.Name,
                        Players = filteredLandings.Select(s => new LiveVeichleTeamPlayers()
                        {
                            PlayerName = s.Character.Name,
                            PlayerId = players.FirstOrDefault(p => p.PlayerName == s.Character.Name)?.PlayerId,
                            location = new LiveLocation()
                            {
                                X = s.Character.Location.x,
                                Y = s.Character.Location.y,
                                Z = s.Character.Location.z,
                            }
                        })
                    });
                }
            }
            response.Landing = landing;

            return response;
        }
        public async Task<IEnumerable<Team>> GetTeams()
        {
            return _team.GetAll("Team").Result;
        }
        public void PostTeam(Team team)
        {
            _team.Insert(team, "Team");
        }
     
        public void DeleteTeam(string teamid)
        {
            var filter = Builders<Team>.Filter.Eq(x => x.Id, teamid);
            _team.DeleteOne(filter,"Team");

        }
        public void UpdatemanyTeams(IEnumerable<Team> teams)
        {
            var teamdetails = _team.GetMongoDbCollection("Team");
            foreach (var team in teams)
            {
                var document = teamdetails.Find(Builders<Team>.Filter.Where(x => x.Id== team.Id)).FirstOrDefault();
                var filter = Builders<Team>.Filter.Eq(s => s.Id, team.Id);
                _team.Replace(team, filter, "Team");
            }

        }

        public void DeleteAll()
        {
            var filter = FilterDefinition<Team>.Empty;
            _team.DeleteMany(filter, "Team");
        }

        public async Task<int> GetTeamCount()
        {
            var cacheKey = "TeamCountCache";

            var teamPlayerFromCache = _cacheService.RetrieveFromCache<int>(cacheKey);

            if (teamPlayerFromCache != 0)
            {
                _logger.LogInformation("TeamPlayer returned from " + cacheKey + Environment.NewLine);

                return teamPlayerFromCache;
            }
            else
            {

                _logger.LogInformation("TeamPlayer Repository call started" + Environment.NewLine);

                var teams = _team.GetAll("Team").Result;

                //var distinctTeamPlayers = players.GroupBy(o => new { o.PlayerName, o.PubgAccountId }).Select(o => o.FirstOrDefault());

                var teamCount = teams.Count();

                _logger.LogInformation("TeamPlayer Results stored to the " + cacheKey + Environment.NewLine);

                await _cacheService.SaveToCache<int>(cacheKey, teamCount, 1800000, 7);

                _logger.LogInformation("TeamPlayer Repository call Ended" + Environment.NewLine);

                return await Task.FromResult(teamCount);
            }
        }
        public async Task <IEnumerable<TeamProfile>> GetAccumulatedTeamStats()
        {
            var OverallTeamStats = this.GetAllTeamStats().Result;
            var webclient = new WebClient();
            var json = webclient.DownloadString(@"Json-folder/Phase1_OverallTeamStats.json");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeamProfile>>(json);
            var sum = OverallTeamStats.Join(result,
 innerKey => innerKey.TeamId, outerkey => outerkey.TeamId, (phase1, phase2) => new TeamProfile()
 {
     TeamId = phase1.TeamId,
     Region=phase1.Region,
     NumMatches = phase1.NumMatches + phase2.NumMatches,
     Name = phase1.Name,
     ShortName = phase1.ShortName,
     stats = new Stats()
     {
         Knocks = phase1.stats.Knocks + phase2.stats.Knocks,
         Assists = phase1.stats.Assists + phase2.stats.Assists,
         Kills = phase1.stats.Kills + phase2.stats.Kills,
         headShot = phase1.stats.headShot + phase2.stats.headShot,
         Heals = phase1.stats.Heals + phase2.stats.Heals,
         damage = phase1.stats.damage + phase2.stats.damage,
         Revives = phase1.stats.Revives + phase2.stats.Revives,
         TimeSurvived = phase1.stats.TimeSurvived + phase2.stats.TimeSurvived,
         Boosts = phase1.stats.Boosts + phase2.stats.Boosts,
         WalkDistance = phase1.stats.WalkDistance + phase2.stats.WalkDistance,
         RideDistance = phase1.stats.RideDistance + phase2.stats.RideDistance,
         SwimDistance = phase1.stats.SwimDistance + phase2.stats.SwimDistance




     }



 });
            return await Task.FromResult(sum);

        }
        public async Task<IEnumerable<TeamProfile>> GetAccumulatedTeamAverageStats()
        {
            var TotalStats = this.GetAccumulatedTeamStats().Result.ToList();
            var AverageTeamStats = TotalStats.GroupBy(g => g.TeamId).Select(s => new TeamProfile
            {
                TeamId = s.Key,
                Name = s.Select(a => a.Name).ElementAtOrDefault(0),
                Region = s.Select(a => a.Region).ElementAtOrDefault(0),
                ShortName = s.Select(a => a.ShortName).ElementAtOrDefault(0),
                NumMatches = s.Select(a => a.NumMatches).ElementAtOrDefault(0),

                stats = new Stats()
                {
                    Knocks = Math.Round((double)(s.Sum(a => a.stats.Knocks) /(double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    Assists = Math.Round((double)(s.Sum(a => a.stats.Assists) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    Boosts = Math.Round((double)(s.Sum(a => a.stats.Boosts) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)),2, MidpointRounding.AwayFromZero),
                    damage = Math.Round((double)(s.Sum(a => a.stats.damage) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    headShot = Math.Round((double)(s.Sum(a => a.stats.headShot) / (double)s.Select(a=>a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    Heals = Math.Round((double)(s.Sum(a => a.stats.Heals) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    Kills = Math.Round((double)(s.Sum(a => a.stats.Kills) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    TimeSurvived = Math.Round((double)(s.Sum(a => a.stats.TimeSurvived) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    Revives = Math.Round((double)(s.Sum(a => a.stats.Revives) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    RideDistance = Math.Round((double)(s.Sum(a => a.stats.RideDistance) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round((double)(s.Sum(a => a.stats.SwimDistance) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round((double)(s.Sum(a => a.stats.WalkDistance) / (double)s.Select(a => a.NumMatches).ElementAtOrDefault(0)), 2, MidpointRounding.AwayFromZero)
                }
            }).OrderBy(o => o.TeamId);


            return await Task.FromResult(AverageTeamStats);
        }
        

    }
}