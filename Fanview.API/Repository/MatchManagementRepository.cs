using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fanview.API.Utility;

namespace Fanview.API.Repository
{
    public class MatchManagementRepository : IMatchManagementRepository
    {
        private IGenericRepository<Event> _tournamentRepository;
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<LiveEventKill> _liveEventKillRepository;
        private IGenericRepository<MatchRanking> _matchRankingRepository;
        private IGenericRepository<Kill> _killRepostiory;
        private IGenericRepository<EventDamage> _eventDamageRepository;
        private readonly IGenericRepository<LiveMatchStatus> _liveMatchStatusRepository;
        private IGenericRepository<MatchPlayerStats> _matchPlayerStatsRepository;
        private IGenericRepository<MatchSafeZone> _matchSafeZoneRepository;
        private readonly IGenericRepository<CreatePlayer> _createPlayerRepository;
        private IGenericRepository<PlayerPoition> _playerPositionRepository;
        private readonly IGenericRepository<TeamRanking> _teamRankingRepository;
        private IGenericRepository<VehicleLeave> _vehicleLeaveRepository;
        private ILogger<MatchManagementRepository> _logger;

        public MatchManagementRepository(IGenericRepository<Event> tournamentRepository, IClientBuilder httpClientBuilder,
                                    IHttpClientRequest httpClientRequest, IGenericRepository<LiveEventKill> liveEventKillRepository, 
                                    IGenericRepository<MatchRanking> matchRankingRepository, IGenericRepository<Kill> killRepostiory,
                                    IGenericRepository<EventDamage> eventDamageRepository, IGenericRepository<LiveMatchStatus> liveMatchStatusRepository,
                                    IGenericRepository<MatchPlayerStats> matchPlayerStatsRepository, IGenericRepository<MatchSafeZone> matchSafeZoneRepository,
                                    IGenericRepository<CreatePlayer> createPlayerRepository, IGenericRepository<PlayerPoition> playerPositionRepository,
                                    IGenericRepository<TeamRanking> teamRankingRepository, IGenericRepository<VehicleLeave> vehicleLeaveRepository,
                                    ILogger<MatchManagementRepository> logger)
        {
            _tournamentRepository = tournamentRepository;
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;
            _liveEventKillRepository = liveEventKillRepository;
            _matchRankingRepository = matchRankingRepository;
            _killRepostiory = killRepostiory;
            _eventDamageRepository = eventDamageRepository;
            _liveMatchStatusRepository = liveMatchStatusRepository;
            _matchPlayerStatsRepository = matchPlayerStatsRepository;
            _matchSafeZoneRepository = matchSafeZoneRepository;
            _createPlayerRepository = createPlayerRepository;
            _playerPositionRepository = playerPositionRepository;
            _teamRankingRepository = teamRankingRepository;
            _vehicleLeaveRepository = vehicleLeaveRepository;
            _logger = logger;
        }

        public dynamic DeleteDocumentCollections(string matchId)
        {  
            var matchRankingFilter = Builders<MatchRanking>.Filter.Eq(s => s.MatchId, matchId);

            var matchRankingResponse = _matchRankingRepository.DeleteMany(matchRankingFilter, "MatchRanking");

            var killFilter = Builders<Kill>.Filter.Eq(s => s.MatchId, matchId);

            var killResponse = _killRepostiory.DeleteMany(killFilter, "Kill");

            var liveEventKillFilter = Builders<LiveEventKill>.Filter.Eq(s => s.MatchId, matchId);

            var liveEventKillResponse = _liveEventKillRepository.DeleteMany(liveEventKillFilter, "LiveEventKill");

            var eventDamageFilter = Builders<EventDamage>.Filter.Empty;

            var eventDamageResponse = _eventDamageRepository.DeleteMany(eventDamageFilter, "LiveEventDamage");

            var liveMatchStatusFilter = Builders<LiveMatchStatus>.Filter.Eq(s => s.MatchId, matchId);

            var liveMatchStatusResponse = _liveMatchStatusRepository.DeleteMany(liveMatchStatusFilter, "TeamLiveStatus");

            var matchPlayerStatsFilter = Builders<MatchPlayerStats>.Filter.Eq(s => s.MatchId, matchId);

            var matchPlayerStatsResponse = _matchPlayerStatsRepository.DeleteMany(matchPlayerStatsFilter, "MatchPlayerStats");

            var matchSafeZoneFilter = Builders<MatchSafeZone>.Filter.Eq(s => s.MatchId, matchId);

            var matchSafeZoneResponse = _matchSafeZoneRepository.DeleteMany(matchSafeZoneFilter, "MatchSafeZone");

            var playerCreatedFilter = Builders<CreatePlayer>.Filter.Eq(s => s.MatchId, matchId);

            var playerCreatedResponse = _createPlayerRepository.DeleteMany(playerCreatedFilter, "PlayerCreated");

            var playerPoitionFilter = Builders<PlayerPoition>.Filter.Eq(s => s.MatchId, matchId);

            var playerPoitionResponse = _playerPositionRepository.DeleteMany(playerPoitionFilter, "PlayerPosition");

            var teamRankingFilter = Builders<TeamRanking>.Filter.Eq(s => s.MatchId, matchId);

            var teamRankingResponse = _teamRankingRepository.DeleteMany(teamRankingFilter, "TeamRanking");

            var vehicleLeaveFilter = Builders<VehicleLeave>.Filter.Eq(s => s.MatchId, matchId);

            var vehicleLeaveResponse = _vehicleLeaveRepository.DeleteMany(vehicleLeaveFilter, "VehicleLeave");

            var tournamentMatchIdFilter = Builders<Event>.Filter.Eq(s => s.Id, matchId);

            var tournamentMatchIdResponse = _tournamentRepository.DeleteOne(tournamentMatchIdFilter, "TournamentMatchId");

            dynamic mongoDeletedCollection = new ExpandoObject();

            List<dynamic> deletedColletionInfo = new List<dynamic>() {
                new ExpandoObject().Init(
                    "CollectionName".WithValue("MatchRanking"),
                    "DocumentDeleteCount".WithValue(matchRankingResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(matchRankingResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("Kill"),
                    "DocumentDeleteCount".WithValue(killResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(killResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("LiveEventKill"),
                    "DocumentDeleteCount".WithValue(liveEventKillResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(liveEventKillResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("LiveEventDamage"),
                    "DocumentDeleteCount".WithValue(eventDamageResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(eventDamageResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("TeamLiveStatus"),
                    "DocumentDeleteCount".WithValue(liveMatchStatusResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(liveMatchStatusResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("MatchPlayerStats"),
                    "DocumentDeleteCount".WithValue(matchPlayerStatsResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(matchPlayerStatsResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("MatchSafeZone"),
                    "DocumentDeleteCount".WithValue(matchSafeZoneResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(matchSafeZoneResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("PlayerCreated"),
                    "DocumentDeleteCount".WithValue(playerCreatedResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(playerCreatedResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("PlayerPoition"),
                    "DocumentDeleteCount".WithValue(playerPoitionResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(playerPoitionResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("TeamRanking"),
                    "DocumentDeleteCount".WithValue(teamRankingResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(teamRankingResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("VehicleLeave"),
                    "DocumentDeleteCount".WithValue(vehicleLeaveResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(vehicleLeaveResponse.IsCompletedSuccessfully)),
                new ExpandoObject().Init(
                    "CollectionName".WithValue("TournamentMatchId"),
                    "DocumentDeleteCount".WithValue(tournamentMatchIdResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(tournamentMatchIdResponse.IsCompletedSuccessfully))
            };

            mongoDeletedCollection.Result = deletedColletionInfo;

            return mongoDeletedCollection;
        }

        public async Task<IEnumerable<Event>> GetMatchDetails()
        {
            var matchCollection = _tournamentRepository.GetMongoDbCollection("TournamentMatchId");

            return await matchCollection.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync();
        }

        public async Task<Object> GetTournaments()
        {
            try
            {
                _logger.LogInformation("Tournament Open API Request Started" + Environment.NewLine);

                var tournamentsResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "tournaments");

                if (tournamentsResponse.Result.StatusCode == HttpStatusCode.OK && tournamentsResponse != null)
                {
                    _logger.LogInformation("Tournament Open API Response Json" + Environment.NewLine);

                    var response = tournamentsResponse.Result.Content.ReadAsStringAsync().Result;

                    var o = JObject.Parse(response);

                    var tournaments = o.SelectToken("data").Select(m =>
                                                    new { Name =  (string)m.SelectToken("id"),
                                                         CreatedDate = (string)m.SelectToken("attributes.createdAt") }
                                                    ).OrderByDescending(a => a.CreatedDate).ToList();

                    return tournaments;
                }
                else
                {
                    return tournamentsResponse.Result;
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> GetTournamentsMatches(string tournamentName)
        {
            try
            {
                _logger.LogInformation("Tournament Open API Request Started" + Environment.NewLine);

                var tournamentsResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "tournaments/" + tournamentName);

                if (tournamentsResponse.Result.StatusCode == HttpStatusCode.OK && tournamentsResponse != null)
                {
                    _logger.LogInformation("Tournament Match Open API Response Json" + Environment.NewLine);

                    var response = tournamentsResponse.Result.Content.ReadAsStringAsync().Result;

                    var o = JObject.Parse(response);

                    var tournaments = o.SelectToken("included").Select(m =>
                                                    new
                                                    {
                                                        MatchId = (string)m.SelectToken("id"),
                                                        CreatedDate = (string)m.SelectToken("attributes.createdAt")
                                                    }
                                                    ).OrderByDescending(a => a.CreatedDate).ToList();

                    return tournaments;
                }
                else
                {
                    return tournamentsResponse.Result;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostMatchDetails(Event matchDetails)
        {
            _tournamentRepository.Insert(matchDetails, "TournamentMatchId");
           
        }
    }
}
