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
using Fanview.API.Services.Interface;

namespace Fanview.API.Repository
{
    public class EventRepository : IEventRepository
    {
       
        private ICacheService _cacheService;
        private IGenericRepository<Event> _tournamentRepository;
        private ILogger<EventRepository> _logger;
        private IMongoCollection<Event> tournamentsDb;

        public EventRepository( ICacheService cacheService, IGenericRepository<Event> tournamentRepository, ILogger<EventRepository> logger)
        {   
            _cacheService = cacheService;
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
            var cacheKey = "TournamentMatchCache";

            var tournamentMatchFromCache = _cacheService.RetrieveFromCache<Event>(cacheKey);

            if (tournamentMatchFromCache != null)
            {
                _logger.LogInformation("tournament Match returned from " + cacheKey + Environment.NewLine);

                return tournamentMatchFromCache;
            }
            else
            {
                _logger.LogInformation("FindEvent Event Repository call started" + Environment.NewLine);

                var tournamentMatch = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.Id == matchId)).Result.FirstOrDefaultAsync().Result;

                _logger.LogInformation("tournament Match Results stored to the " + cacheKey + Environment.NewLine);

                await _cacheService.SaveToCache<Event>(cacheKey, tournamentMatch, 45, 7);

                _logger.LogInformation("FindEvent Event Repository call Ended" + Environment.NewLine);

                return await Task.FromResult(tournamentMatch);
            }

                
        }

        public async Task<string> GetEventCreatedAt(int matchId)
        {
            var cacheKey = "TournamentMatchCreatedAtCache";

            try
            {
                var tournamentMatchFromCache = _cacheService.RetrieveFromCache<string>(cacheKey);

                if (tournamentMatchFromCache != null)
                {                

                    return tournamentMatchFromCache;
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation("TournamentMatchCreatedAtCache exception" + ex + Environment.NewLine);
            }

           
           
            _logger.LogInformation("GetEventCreatedAT Event Repository call started" + Environment.NewLine);

            var tournamentMatchCreateAt = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.CreatedAT;

            _logger.LogInformation("tournament MatchcreatedAt Results stored to the " + cacheKey + Environment.NewLine);

            await _cacheService.SaveToCache<string>(cacheKey, tournamentMatchCreateAt, 45, 7);

            _logger.LogInformation("GetEventCreatedAT Event Repository call Ended" + Environment.NewLine);

            return await Task.FromResult(tournamentMatchCreateAt);
        
        }

        public Task<int> GetTournamentMatchCount()
        {
            return Task.FromResult(tournamentsDb.FindAsync(new BsonDocument()).Result.ToListAsync().Result.Count());
        }

        public async Task<string> GetTournamentLiveMatch()
        {
            var cacheKey = "TournamentMatchIdCache";

            try
            { 
                var tournamentMatchFromCache =  _cacheService.RetrieveFromCache<string>(cacheKey);

                if (tournamentMatchFromCache != null)
                {
                    return tournamentMatchFromCache;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("TournamentMatchIdCache exception" + ex + Environment.NewLine);
            }
            
            
            _logger.LogInformation("GetTournamentMatchIdEvent Repository call started" + Environment.NewLine);

            _cacheService.RemoveFromCache();

            _cacheService.RefreshFromCache();

            var tournamentMatchId = tournamentsDb.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync().Result.LastOrDefault().Id;

            _logger.LogInformation("tournament MatchId Results stored to the " + cacheKey + Environment.NewLine);
                       
            await _cacheService.SaveToCache<string>(cacheKey, tournamentMatchId, 30, 5);           

            _logger.LogInformation("GetTournamentMatchIdEvent Repository call Ended" + Environment.NewLine);

            return await Task.FromResult(tournamentMatchId);
        }

        public async Task<string> GetTournamentMatchIdNotCached(int matchId)
        {
           
            var tournamentMatchId = tournamentsDb.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            return await Task.FromResult(tournamentMatchId);
        }
    }
}
