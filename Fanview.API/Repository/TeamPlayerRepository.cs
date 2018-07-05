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

        public TeamPlayerRepository(IGenericRepository<TeamPlayer> genericRepository, ILogger<TeamRepository> logger)
        {
            _genericTeamPlayerRepository = genericRepository;

            _logger = logger;
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

        public async void InsertTeamPlayer(TeamPlayer teamPlayer)
        {
            Func<Task> persistDataToMongo = async () => _genericTeamPlayerRepository.Insert(teamPlayer, "TeamPlayers");

            await Task.Run(persistDataToMongo);
        }
    }
}
