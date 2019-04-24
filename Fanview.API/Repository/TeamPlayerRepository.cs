using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class TeamPlayerRepository : ITeamPlayerRepository
    {
        private IGenericRepository<TeamPlayer> _genericTeamPlayerRepository;
        private ILogger<TeamRepository> _logger;
        private IGenericRepository<Team> _genericTeamRepository;
        private IGenericRepository<CreatePlayer> _genericPlayerRepository;
        private IGenericRepository<MatchPlayerStats> _genericMatchPlayerStatsRepository;
        private IGenericRepository<Event> _tournament;
        private IGenericRepository<PlayerProfileTournament> _playerstates;

        public TeamPlayerRepository(IGenericRepository<TeamPlayer> genericRepository, ILogger<TeamRepository> logger,
            IGenericRepository<Team> teamgenericRepository,
            IGenericRepository<CreatePlayer> genericPlayerRepository,
            IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
            IGenericRepository<Event> tournament,
            IGenericRepository<PlayerProfileTournament> playerstates)
        {
            _genericTeamPlayerRepository = genericRepository;

            _logger = logger;
            _genericTeamRepository= teamgenericRepository;
            _genericPlayerRepository = genericPlayerRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            _tournament = tournament;
            _playerstates = playerstates;
        }

        public async Task<TeamPlayer> GetPlayerProfile(string playerId1)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.Id == playerId1)).Result.SingleOrDefaultAsync();

            return teamPlayer;
        }
        
        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers()
        {
            var collection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");
            var filter = Builders<TeamPlayer>.Filter.Empty;
            var options = new FindOptions<TeamPlayer>()
            {
                Sort = Builders<TeamPlayer>.Sort.Descending("teamIdShort").Descending("fullName")
            };
            var query = await collection.FindAsync(filter, options).ConfigureAwait(false);
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TeamLineUp> GetTeamandPlayers()
        {
            var teamPlayers = await _genericTeamPlayerRepository.GetAll("TeamPlayers");

            var teamplayerlist = teamPlayers.ToList();

            var teams = await _genericTeamRepository.GetAll("Team");

            var teamlist = teams;

            var unique = teamPlayers.GroupBy(o => new { o.PlayerName, o.PubgAccountId }).Select(o => o.FirstOrDefault());

            var query = teamlist.GroupJoin(unique, tp => tp.Id, t => t.TeamId, (t, tp) => new
            {
                TeamId = t.Id,
                TeamName = t.Name,
                TeamPlayers = tp.Select(s => s.PlayerName)
            });

            var teamlineup = new TeamLineUp();

            var teamlineplayers = new List<TeamLineUpPlayers>();

            foreach (var teamplayer in query)
            {
                teamlineup.TeamId = teamplayer.TeamId;

                teamlineup.TeamName = teamplayer.TeamName;

                foreach (var player in teamplayer.TeamPlayers)
                {
                    teamlineplayers.Add(new TeamLineUpPlayers { PlayerName = player });
                }
                teamlineup.TeamPlayer = teamlineplayers;
            }

            var result = teamlineup;

            return result;

        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.AsQueryable().ToListAsync();

            return teamPlayer;
        }

        public async void InsertTeamPlayer(TeamPlayer teamPlayer)
        {
            Func<Task> persistDataToMongo = async () => _genericTeamPlayerRepository.Insert(teamPlayer, "TeamPlayers");

            await Task.Run(persistDataToMongo);
        }

        public async Task<IEnumerable<CreatePlayer>> GetPlayersCreated(string matchId)
        {
            var playerCreatedCollection = _genericPlayerRepository.GetMongoDbCollection("PlayerCreated");

            var playerCreated = await playerCreatedCollection.FindAsync(Builders<CreatePlayer>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            return playerCreated;
        }

        public async Task<List<PlayerProfileTournament>> GetPlayerTournamentStats()
        {
            
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Empty).Result.ToListAsync();

            var matchstats =_genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;
            var matchstat = matchstats.AsQueryable().ToList();
            var matchcount = matchstats.Select(x => x.MatchId).Distinct().Count();
            var playerProfile = matchstats.Join(teamPlayer, ms => ms.stats.Name.Trim(), tp => tp.PlayerName.Trim(), (ms, tp) => new { ms, tp })
                                          .Select(s => new
                                          {
                                              MatchId = s.ms.MatchId,
                                              PlayerId = s.tp.PlayerId,
                                              PlayerName = s.tp.PlayerName,
                                              FullName = s.tp.FullName,
                                              Country = s.tp.Country,
                                              TeamId = s.tp.TeamIdShort,
                                              Stats = new
                                              {   Knocs = s.ms.stats.DBNOs,
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
           var matchcoun = playerProfile.GroupBy(x=>x.PlayerId).Select(group=>new { matchid = group.Select(x => x.MatchId) }).Select(x=>x.matchid.Count());
            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {
                NumMatches = s.Select(c => c.MatchId).Count(),
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                TeamId = s.Select(c => c.TeamId).ElementAtOrDefault(0),
                
           
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
            }).OrderBy(o => o.PlayerId).ToList();
            var detailed = PlayerProfileGrouped.ToList();
           
            return PlayerProfileGrouped;
        }
        public async Task<List<PlayerProfileTournament>> GetPlayerTournamentAverageStats()
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Empty).Result.ToListAsync();

            var matchstats = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;
            //var matchstat = matchstats.AsQueryable().ToList();
            //var matchcount = matchstats.Select(x => x.MatchId).Distinct().Count();
            var playerProfile = matchstats.Join(teamPlayer, ms => ms.stats.Name.Trim(), tp => tp.PlayerName.Trim(), (ms, tp) => new { ms, tp })
                                          .Select(s => new
                                          {
                                              MatchId = s.ms.MatchId,
                                              PlayerId = s.tp.PlayerId,
                                              PlayerName = s.tp.PlayerName,
                                              FullName = s.tp.FullName,
                                              Country = s.tp.Country,
                                              TeamId = s.tp.TeamIdShort,
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
            //var matchcoun = playerProfile.GroupBy(x => x.PlayerName).Select(group => new { PlayerName = group.Key, matchid = group.Select(x => x.MatchId) }).ToDictionary(x => x.PlayerName, x => x.matchid.Count());
            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {
                NumMatches = s.Select(c => c.MatchId).Count(),
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                TeamId = s.Select(c => c.TeamId).ElementAtOrDefault(0),
                
                stats = new Stats()
                {
                    
                    Knocks = Math.Round(s.Average(a => a.Stats.Knocs), 2, MidpointRounding.AwayFromZero),
                    Assists = Math.Round(s.Average(a => a.Stats.Assists) , 2, MidpointRounding.AwayFromZero),
                    Boosts = Math.Round(s.Average(a => a.Stats.Boosts), 2, MidpointRounding.AwayFromZero),
                    damage = Math.Round(s.Average(a => a.Stats.Damage), 2, MidpointRounding.AwayFromZero),
                    headShot = Math.Round(s.Average(a => a.Stats.HeadShort) , 2, MidpointRounding.AwayFromZero),
                    Heals = Math.Round(s.Average(a => a.Stats.Heals) , 2, MidpointRounding.AwayFromZero),
                    Kills = Math.Round(s.Average(a => a.Stats.Kills) , 2, MidpointRounding.AwayFromZero),
                    TimeSurvived = Math.Round(s.Average(a => a.Stats.TimeSurvived) , 2, MidpointRounding.AwayFromZero),
                    Revives = Math.Round(s.Average(a => a.Stats.Revives), 2, MidpointRounding.AwayFromZero),
                    RideDistance = Math.Round(s.Average(a => a.Stats.RideDistance) , 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round(s.Average(a => a.Stats.SwimDistance), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round(s.Average(a => a.Stats.WalkDistance) , 2, MidpointRounding.AwayFromZero)

                }
            }).OrderBy(o => o.PlayerId);
            return PlayerProfileGrouped.ToList();
        }

        public async Task<Object> GetPlayerTournamentStats(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.AsQueryable().ToListAsync();

            var matchstats = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");

            var matchstat = await matchstats.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync();

            var playerProfile = matchstat.Join(teamPlayer, ms => ms.stats.Name, tp => tp.PlayerName, (ms, tp) => new { ms, tp })                                          
                                          .Select(s => new
                                          {   
                                              MatchId = s.tp.MatchId,
                                              PlayerId = s.tp.PlayerId,
                                              PlayerName = s.tp.PlayerName,
                                              FullName = s.tp.FullName,
                                              Country = s.tp.Country,
                                              TeamId = s.tp.TeamIdShort,
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

            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {
                MatchId = s.Select(c=>c.MatchId).ElementAtOrDefault(0),
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                TeamId = s.Select(c => c.TeamId).ElementAtOrDefault(0),
                NumMatches=s.Select(c=>c.MatchId).Count(),
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
            }).OrderBy(o => o.PlayerId);
            _playerstates.Insert(PlayerProfileGrouped, "Phase1AveragePlayerStats");
            return PlayerProfileGrouped;
        }


        public async Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersStatsMatchUp(int playerId1, int playerId2, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => (cn.PlayerId == playerId1 || cn.PlayerId == playerId2) 
                                                        )).Result.ToListAsync();
           

            var matchstats = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");

            var matchstat = await matchstats.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync();

            var playerProfile = matchstat.Join(teamPlayer, ms => ms.stats.Name.Trim(), tp => tp.PlayerName.Trim(), (ms, tp) => new { ms, tp })                                          
                                          .Select(s => new
                                          {
                                              MatchId = s.tp.MatchId,
                                              PlayerId = s.tp.PlayerId,
                                              PlayerName = s.tp.PlayerName,
                                              FullName = s.tp.FullName,
                                              Country = s.tp.Country,
                                              TeamId = s.tp.TeamIdShort,
                                              Stats = new
                                              {
                                                  Knocs = s.ms.stats.DBNOs,
                                                  Assists = s.ms.stats.Assists,
                                                  Boosts = s.ms.stats.Boosts,
                                                  Damage = s.ms.stats.DamageDealt,
                                                  HeadShort = s.ms.stats.HeadshotKills,
                                                  Heals = s.ms.stats.Heals,
                                                  Kills = s.ms.stats.Kills,
                                                  TimeSurvived = s.ms.stats.TimeSurvived
                                              }
                                          });

            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {
                
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                stats = new Stats()
                {
                    Knocks = s.Sum(a => a.Stats.Knocs),
                    Assists = s.Sum(a => a.Stats.Assists),
                    Boosts = s.Sum(a => a.Stats.Boosts),
                    damage = s.Sum(a => a.Stats.Damage),
                    headShot = s.Sum(a => a.Stats.HeadShort),
                    Heals = s.Sum(a => a.Stats.Heals),
                    Kills = s.Sum(a => a.Stats.Kills),
                    TimeSurvived = s.Sum(a => a.Stats.TimeSurvived)
                }
            });

            return PlayerProfileGrouped;
        }

        public async Task<IEnumerable<PlayerProfileTournament>> GetPlayerProfilesMatchUP(int playerId1, int playerId2)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.PlayerId == playerId1 || cn.PlayerId == playerId2)).Result.ToListAsync();

            var matchstats = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;

            var playerProfile = matchstats.Join(teamPlayer, ms => ms.stats.Name, tp => tp.PlayerName, (ms, tp) => new { ms, tp })                                          
                                          .Select(s => new
                                          {
                                              MatchId = s.tp.MatchId,
                                              PlayerId = s.tp.PlayerId,
                                              PlayerName = s.tp.PlayerName,
                                              FullName = s.tp.FullName,
                                              Country = s.tp.Country,
                                              TeamId = s.tp.TeamIdShort,
                                              Stats = new
                                              {
                                                  Knocs = s.ms.stats.DBNOs,
                                                  Assists = s.ms.stats.Assists,
                                                  Boosts = s.ms.stats.Boosts,
                                                  Damage = s.ms.stats.DamageDealt,
                                                  HeadShort = s.ms.stats.HeadshotKills,
                                                  Heals = s.ms.stats.Heals,
                                                  Kills = s.ms.stats.Kills,
                                                  TimeSurvived = s.ms.stats.TimeSurvived
                                              }
                                          });


            var i = 0;

            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {   
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                TeamId = s.Select(c => c.TeamId).ElementAtOrDefault(i++),
                stats = new Stats()
                {
                    Knocks = s.Sum(a => a.Stats.Knocs),
                    Assists = s.Sum(a => a.Stats.Assists),
                    Boosts = s.Sum(a => a.Stats.Boosts),
                    damage = s.Sum(a => a.Stats.Damage),
                    headShot = s.Sum(a => a.Stats.HeadShort),
                    Heals = s.Sum(a => a.Stats.Heals),
                    Kills = s.Sum(a => a.Stats.Kills),
                    TimeSurvived = s.Sum(a => a.Stats.TimeSurvived)
                }
            });

            return PlayerProfileGrouped;
        }
        
        public void CreateNewPlayer(TeamPlayerViewModel player)
        {
            var newPlayer = new TeamPlayer()
            {
                TeamId = player.TeamId,
                TeamIdShort = player.TeamIdShort,
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                FullName  = player.FullName,
                Country = player.Country
                
            };
                
            _genericTeamPlayerRepository.Insert(newPlayer, "TeamPlayers");
        }
        public void Deleteplayer(string playerid)
        {
            var filter = Builders<TeamPlayer>.Filter.Eq(x => x.Id, playerid);
            _genericTeamPlayerRepository.DeleteOne(filter, "TeamPlayers");

        }
        
        
        public void Updatemanyplayers(IEnumerable<TeamPlayer> players)
        {
            var playersdetails = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");
            foreach (var player in players)
            {
                var document = playersdetails.Find(Builders<TeamPlayer>.Filter.Where(x => x.Id == player.Id)).FirstOrDefault();
                var filter = Builders<TeamPlayer>.Filter.Eq(s => s.Id, player.Id);
                _genericTeamPlayerRepository.Replace(player, filter, "TeamPlayers");
            }

        }
        public void DeleteAllTeamPlayers()
        {
            var filter = Builders<TeamPlayer>.Filter.Empty;
            _genericTeamPlayerRepository.DeleteMany(filter, "TeamPlayers");
        }

        public async Task<IEnumerable<PlayerKilledGraphics>> GetPlayersId(IEnumerable<LiveEventKill> liveEventKills)
        {
            var teamPlayers =  await GetTeamPlayers().ConfigureAwait(false);

            var liveKilledPlayersVictim = liveEventKills.Join(teamPlayers, pk => pk.VictimName.ToLower().Trim(), tp => tp.PlayerName.ToLower().Trim(), (pk, tp) => new { pk, tp });

            var liveKilledOuterJoin = liveKilledPlayersVictim.GroupJoin(teamPlayers, left => left.pk.KillerName.ToLower().Trim(), right => right.PlayerName.ToLower().Trim(),
                                                            (left, right) => new { TableA = right, TableB = left }).SelectMany(p => p.TableA.DefaultIfEmpty(), (x, y) => new { TableA = y, TableB = x.TableB });

            var liveKilledPlayers = new List<PlayerKilledGraphics>();

            foreach (var item in liveKilledOuterJoin)
            {
                if (item.TableA != null && item.TableB != null)
                {
                    liveKilledPlayers.Add(new PlayerKilledGraphics()
                    {
                        TimeKilled = item.TableB.pk.EventTimeStamp,
                        KillerName = item.TableB.pk.KillerName,
                        VictimName = item.TableB.pk.VictimName,
                        VictimLocation = item.TableB.pk.VictimLocation,
                        DamagedCausedBy = item.TableB.pk.DamageCauser,
                        DamageReason = item.TableB.pk.DamageReason,
                        VictimTeamId = item.TableB.pk.VictimTeamId,
                        KillerTeamId = item.TableB.pk.KillerTeamId,
                        IsGroggy = item.TableB.pk.IsGroggy,
                        VictimPlayerId = item.TableB.tp.PlayerId,
                        KillerPlayerId = item.TableA.PlayerId,
                    });
                }
                else
                {
                    liveKilledPlayers.Add(new PlayerKilledGraphics()
                    {
                        TimeKilled = item.TableB.pk.EventTimeStamp,
                        KillerName = item.TableB.pk.KillerName,
                        VictimName = item.TableB.pk.VictimName,
                        VictimLocation = item.TableB.pk.VictimLocation,
                        DamagedCausedBy = item.TableB.pk.DamageCauser,
                        DamageReason = item.TableB.pk.DamageReason,
                        VictimTeamId = item.TableB.pk.VictimTeamId,
                        KillerTeamId = item.TableB.pk.KillerTeamId,
                        IsGroggy = item.TableB.pk.IsGroggy,
                        VictimPlayerId = item.TableB.tp.PlayerId,
                        KillerPlayerId = 0,
                    });
                }
            }
            return liveKilledPlayers;          
            
        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayersNonCached()
        {
            _logger.LogInformation("TeamPlayer Repository call started" + Environment.NewLine);

            var players = _genericTeamPlayerRepository.GetAll("TeamPlayers").Result;           

            var teamPlayers = players.OrderBy(o => o.TeamIdShort).ThenBy(t => t.FullName);            

            _logger.LogInformation("TeamPlayer Repository call Ended" + Environment.NewLine);

            return await Task.FromResult(teamPlayers);
        }
        public async Task<IEnumerable<PlayerProfileTournament>> AccumulateOverallPlayerStats()
        {
            var OveralStats = this.GetPlayerTournamentStats().Result;
            var webclient = new WebClient();
            var json = webclient.DownloadString(@"Json-folder\Phase1_OverallPlayerStats.json");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlayerProfileTournament>>(json);
            //var Sum=Enumerable.Range(0, Math.Max(OveralStats.Count, phase1Overal.Count))
            //     .Select(i=>
            //     OveralStatsElementAtOrDefault(i)+phase1Overal.ElementAtOrDefault(i))
            var sum = OveralStats.Join(result, 
    innerKey => innerKey.PlayerId,outerkey=>outerkey.PlayerId, (phase1, phase2) => new PlayerProfileTournament()
    {
       TeamId= phase1.TeamId,
       MatchId= phase1.MatchId,
       NumMatches= phase1.NumMatches+phase2.NumMatches,
       PlayerName= phase1.PlayerName,
       PlayerId= phase1.PlayerId,
       Country= phase1.Country,
       FullName= phase1.FullName,
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
            SwimDistance=phase1.stats.SwimDistance+phase2.stats.SwimDistance


            

        }
       
        
        
    });
            var sumlist = sum.ToList();
            return await Task.FromResult(sumlist);   }
        public async Task<IEnumerable<PlayerProfileTournament>> AccumulatedAveragePlayerStats()
        {
            var totalsum = this.AccumulateOverallPlayerStats().Result;
            var TotalAverage = totalsum.GroupBy(g => g.PlayerId).Select(s => new PlayerProfileTournament()
            {
                NumMatches = s.Select(c => c.MatchId).Count(),
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                TeamId = s.Select(c => c.TeamId).ElementAtOrDefault(0),

                stats = new Stats()
                {

                    Knocks = Math.Round(s.Average(a => a.stats.Knocks), 2, MidpointRounding.AwayFromZero),
                    Assists = Math.Round(s.Average(a => a.stats.Assists), 2, MidpointRounding.AwayFromZero),
                    Boosts = Math.Round(s.Average(a => a.stats.Boosts), 2, MidpointRounding.AwayFromZero),
                    damage = Math.Round(s.Average(a => a.stats.damage), 2, MidpointRounding.AwayFromZero),
                    headShot = Math.Round(s.Average(a => a.stats.headShot), 2, MidpointRounding.AwayFromZero),
                    Heals = Math.Round(s.Average(a => a.stats.Heals), 2, MidpointRounding.AwayFromZero),
                    Kills = Math.Round(s.Average(a => a.stats.Kills), 2, MidpointRounding.AwayFromZero),
                    TimeSurvived = Math.Round(s.Average(a => a.stats.TimeSurvived), 2, MidpointRounding.AwayFromZero),
                    Revives = Math.Round(s.Average(a => a.stats.Revives), 2, MidpointRounding.AwayFromZero),
                    RideDistance = Math.Round(s.Average(a => a.stats.RideDistance), 2, MidpointRounding.AwayFromZero),
                    SwimDistance = Math.Round(s.Average(a => a.stats.SwimDistance), 2, MidpointRounding.AwayFromZero),
                    WalkDistance = Math.Round(s.Average(a => a.stats.WalkDistance), 2, MidpointRounding.AwayFromZero)

                }
            }).OrderBy(o => o.PlayerId);

            return await Task.FromResult(TotalAverage);
        }

        }
        }

