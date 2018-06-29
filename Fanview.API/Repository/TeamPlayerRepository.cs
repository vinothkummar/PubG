using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Fanview.API.Repository
{
    public class TeamPlayerRepository : ITeamPlayerRepository
    {
        private IGenericRepository<TeamPlayer> _genericRepository;
        private ILogger<TeamRepository> _logger;

        public TeamPlayerRepository(IGenericRepository<TeamPlayer> genericRepository, ILogger<TeamRepository> logger)
        {
            _genericRepository = genericRepository;

            _logger = logger;
        }
        public async void InsertTeamPlayer(TeamPlayer teamPlayer)
        { 
            Func<Task> persistDataToMongo = async () => _genericRepository.Insert(teamPlayer, "TeamPlayer");

            await Task.Run(persistDataToMongo);
        }
    }
}
