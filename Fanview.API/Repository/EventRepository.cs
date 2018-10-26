using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class EventRepository : IEventRepository
    {
        private IMemoryCache _cache;
        private IGenericRepository<Event> _tournamentRepository;
        private ILogger<EventRepository> _logger;
        private IMongoCollection<Event> tournamentsDb;

        public EventRepository(IMemoryCache cache, IGenericRepository<Event> tournamentRepository, ILogger<EventRepository> logger)
        {
            _cache = cache;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
            tournamentsDb = _tournamentRepository.GetMongoDbCollection("TournamentMatchId");

        }

        public void CreateAnEvent(Event newMatch)
        {
            _tournamentRepository.Insert(newMatch, "TournamentMatchId");
        }

        public async Task<Event> FindEvent(string matchId)
        {
            return await _cache.GetOrCreateAsync<Event>("EventMatch", cacheEntry => {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(7)
                };

                cacheEntry.SetOptions(options);

                _logger.LogInformation("FindEvent Event Repository call started" + Environment.NewLine);

                var tournamentMatch = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.Id == matchId)).Result.FirstOrDefaultAsync().Result;

                _logger.LogInformation("FindEvent Event Repository call Ended" + Environment.NewLine);

                return Task.FromResult(tournamentMatch);
            });
            
        }

        public async Task<string> GetEventCreatedAt(int matchId)
        {
            return await _cache.GetOrCreateAsync<string>("EventMatchCreatedAT", cacheEntry => {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(7)
                };

                cacheEntry.SetOptions(options);

                _logger.LogInformation("GetEventCreatedAT Event Repository call started" + Environment.NewLine);
                
                var tournamentMatchCreateAt = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.CreatedAT;

                _logger.LogInformation("GetEventCreatedAT Event Repository call Ended" + Environment.NewLine);

                return Task.FromResult(tournamentMatchCreateAt);
            });
        }

        public Task<int> GetTournamentMatchCount()
        {
            return Task.FromResult(tournamentsDb.FindAsync(new BsonDocument()).Result.ToListAsync().Result.Count());
        }

        public async Task<string> GetTournamentMatchId(int matchId)
        {
            return await _cache.GetOrCreateAsync<string>("EventMatchId", cacheEntry => {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(7)
                };

                cacheEntry.SetOptions(options);

                _logger.LogInformation("GetTournamentMatchIdEvent Repository call started" + Environment.NewLine);

                var tournamentMatchId = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

                _logger.LogInformation("GetTournamentMatchIdEvent Repository call Ended" + Environment.NewLine);

                return Task.FromResult(tournamentMatchId);
            });
            
        }
    }
}
