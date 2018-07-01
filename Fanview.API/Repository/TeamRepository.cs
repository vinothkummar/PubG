using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private IGenericRepository<Team> _genericRepository;
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IGenericRepository<Team> genericRepository, ILogger<TeamRepository> logger)
        {
            _genericRepository = genericRepository;

            _logger = logger;
        }

        public async Task<IEnumerable<Team>> GetTeam()
        { 
            var response = _genericRepository.GetAll("Team");

            return await response;
        }

        public async void InsertTeam(Team team)
        {
            Func<Task> persistDataToMongo = async () => _genericRepository.Insert(team, "Team");

            await Task.Run(persistDataToMongo);
        }
    }
}
