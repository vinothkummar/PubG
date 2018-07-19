﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
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



        public TeamPlayerRepository(IGenericRepository<TeamPlayer> genericRepository, ILogger<TeamRepository> logger,
            IGenericRepository<Team> teamgenericRepository,
            IGenericRepository<CreatePlayer> genericPlayerRepository,
            IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
            IGenericRepository<Event> tournament
            )
        {
            _genericTeamPlayerRepository = genericRepository;

            _logger = logger;
            _genericTeamRepository= teamgenericRepository;
            _genericPlayerRepository = genericPlayerRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            _tournament = tournament;
            
            
        }

        public async Task<TeamPlayer> GetPlayerProfile(string playerId1)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.Id == playerId1)).Result.SingleOrDefaultAsync();

            return teamPlayer;
        }
        
        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers()
        {
            var players = await _genericTeamPlayerRepository.GetAll("TeamPlayers");

            var distinctTeamPlayers = players.GroupBy(o => new { o.PlayerName, o.PubgAccountId }).Select(o => o.FirstOrDefault());

            return distinctTeamPlayers;
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

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            return teamPlayer;
        }
      
        public async Task<IEnumerable<TeamPlayer>> GetAllTeamPlayer()
        {
            return await _genericTeamPlayerRepository.GetAll("TeamPlayers");
        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.MatchId == matchId1 || cn.MatchId == matchId2 || cn.MatchId == matchId3 || cn.MatchId == matchId4)).Result.ToListAsync();

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

        public async Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersTournament(int playerId)
        {
            
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.PlayerId == playerId)).Result.ToListAsync();

            var matchstats =_genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;

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
                                              {   Knocs = s.ms.stats.DBNOs,
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
                MatchId = s.Select(c => c.MatchId).ElementAtOrDefault(0),
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
                    TimeSruvived = s.Sum(a => a.Stats.TimeSurvived)
                }
            });


           

            return PlayerProfileGrouped;

        }

        public async Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersTournament(int playerId, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.PlayerId == playerId && cn.MatchId == tournamentMatchId)).Result.ToListAsync();

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
                                                  TimeSurvived = s.ms.stats.TimeSurvived
                                              }
                                          });

            var PlayerProfileGrouped = playerProfile.GroupBy(g => g.PlayerName).Select(s => new PlayerProfileTournament()
            {
                MatchId = s.Select(c => c.MatchId).ElementAtOrDefault(0),
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
                    TimeSruvived = s.Sum(a => a.Stats.TimeSurvived)
                }
            });

            return PlayerProfileGrouped;
        }


        public async Task<IEnumerable<PlayerProfileTournament>> GetTeamPlayersStatsMatchUp(int playerId1, int playerId2, int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => (cn.PlayerId == playerId1 || cn.PlayerId == playerId2) 
                                                        && (cn.MatchId == tournamentMatchId))).Result.ToListAsync();
           

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
                MatchId = s.Select(c => c.MatchId).ElementAtOrDefault(0),
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
                    TimeSruvived = s.Sum(a => a.Stats.TimeSurvived)
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
                MatchId = s.Select(c => c.MatchId).ElementAtOrDefault(0),
                PlayerId = s.Select(c => c.PlayerId).ElementAtOrDefault(0),
                PlayerName = s.Select(c => c.PlayerName).ElementAtOrDefault(0),
                FullName = s.Select(c => c.FullName).ElementAtOrDefault(0),
                Country = s.Select(c => c.Country).ElementAtOrDefault(0),
                teamId = s.Select(c => c.TeamId).ElementAtOrDefault(i++),
                stats = new Stats()
                {
                    Knocks = s.Sum(a => a.Stats.Knocs),
                    Assists = s.Sum(a => a.Stats.Assists),
                    Boosts = s.Sum(a => a.Stats.Boosts),
                    damage = s.Sum(a => a.Stats.Damage),
                    headShot = s.Sum(a => a.Stats.HeadShort),
                    Heals = s.Sum(a => a.Stats.Heals),
                    Kills = s.Sum(a => a.Stats.Kills),
                    TimeSruvived = s.Sum(a => a.Stats.TimeSurvived)
                }
            });

            return PlayerProfileGrouped;
        }
    }
}
