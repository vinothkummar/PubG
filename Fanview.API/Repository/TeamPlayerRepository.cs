using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
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

        public TeamPlayerRepository(IGenericRepository<TeamPlayer> genericRepository, ILogger<TeamRepository> logger,
            IGenericRepository<Team> teamgenericRepository,
            IGenericRepository<CreatePlayer> genericPlayerRepository
            )
        {
            _genericTeamPlayerRepository = genericRepository;

            _logger = logger;
            _genericTeamRepository= teamgenericRepository;
            _genericPlayerRepository = genericPlayerRepository;
        }

        public async Task<IEnumerable<TeamPlayer>> GetPlayerMatchup(string playerId1, string playerId2)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayers = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.Id == playerId1 || cn.Id == playerId2)).Result.ToListAsync();

            return teamPlayers;
        }

        public async Task<TeamPlayer> GetPlayerProfile(string playerId1)
        {
            var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            var teamPlayer = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Where(cn => cn.Id == playerId1)).Result.SingleOrDefaultAsync();

            return teamPlayer;
        }
        //I'm changing this part of code
        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayers()
        {
            var teamplayers = await _genericTeamPlayerRepository.GetAll("TeamPlayers");
            var list = teamplayers.ToList();
            var mydistinc = teamplayers.GroupBy(o => new { o.PlayerName, o.PubgAccountId }).Select(o => o.FirstOrDefault()).ToList();
            return mydistinc;
        }
        ////I'm changing this part of code 
        public async Task<TeamLineUp> GetTeamandPlayers()
        {


            var teamPlayers = await _genericTeamPlayerRepository.GetAll("TeamPlayers");
            var teamplayerlist = teamPlayers.ToList();
            var teams = await _genericTeamRepository.GetAll("Team");
            var teamlist = teams.ToList();
            var unique = teamPlayers.GroupBy(o => new { o.PlayerName, o.PubgAccountId }).Select(o => o.FirstOrDefault()).ToList();
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
    }
}
