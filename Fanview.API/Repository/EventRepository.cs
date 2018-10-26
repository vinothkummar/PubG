using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class EventRepository : IEventRepository
    {
        private IMemoryCache _cache;
        private IGenericRepository<Event> _tournamentRepository;
        private IMongoCollection<Event> tournamentsDb;

        public EventRepository(IMemoryCache cache, IGenericRepository<Event> tournamentRepository)
        {
            _cache = cache;
            _tournamentRepository = tournamentRepository;
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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(7)
                };

                cacheEntry.SetOptions(options);

                var tournamentMatch = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.Id == matchId)).Result.FirstOrDefaultAsync().Result;

                return Task.FromResult(tournamentMatch);
            });
            
        }

        public async Task<string> GetEventCreatedAt(int matchId)
        {
            return await _cache.GetOrCreateAsync<string>("EventMatchCreatedAT", cacheEntry => {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(7)
                };

                cacheEntry.SetOptions(options);

                var tournamentMatchCreateAt = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.CreatedAT;

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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(7)
                };

                cacheEntry.SetOptions(options);

                var tournamentMatchId = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

                return Task.FromResult(tournamentMatchId);
            });
            
        }
    }
}
