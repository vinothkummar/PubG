using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class MatchManagementRepository : IMatchManagementRepository
    {
        private IGenericRepository<Event> _tournamentRepository;

        public MatchManagementRepository(IGenericRepository<Event> tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<IEnumerable<Event>> GetMatchDetails()
        {
            var matchCollection = _tournamentRepository.GetMongoDbCollection("TournamentMatchId");

            return await matchCollection.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync();
        }
    }
}
