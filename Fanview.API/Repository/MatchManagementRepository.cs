using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class MatchManagementRepository : IMatchManagementRepository
    {
        private IGenericRepository<Event> _tournamentRepository;
        private IGenericRepository<DeskSeatingPosition> _deskPositionRepository;
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
        private readonly IGenericRepository<ParachuteLanding> _parachuteLandingRepository;
        private IGenericRepository<EventLiveMatchStatus> _eventMatchStatusRepository;
        private ILogger<MatchManagementRepository> _logger;

        public MatchManagementRepository(IGenericRepository<Event> tournamentRepository, IClientBuilder httpClientBuilder,
                                    IHttpClientRequest httpClientRequest, IGenericRepository<LiveEventKill> liveEventKillRepository,
                                    IGenericRepository<MatchRanking> matchRankingRepository, IGenericRepository<Kill> killRepostiory,
                                    IGenericRepository<EventDamage> eventDamageRepository, IGenericRepository<LiveMatchStatus> liveMatchStatusRepository,
                                    IGenericRepository<MatchPlayerStats> matchPlayerStatsRepository, IGenericRepository<MatchSafeZone> matchSafeZoneRepository,
                                    IGenericRepository<CreatePlayer> createPlayerRepository, IGenericRepository<PlayerPoition> playerPositionRepository,
                                    IGenericRepository<TeamRanking> teamRankingRepository, IGenericRepository<VehicleLeave> vehicleLeaveRepository,
                                    IGenericRepository<ParachuteLanding> parachuteLandingRepository,
                                    IGenericRepository<EventLiveMatchStatus> eventMatchStatusRepository,
                                    IGenericRepository<DeskSeatingPosition> deskPositionRepository,
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
            _parachuteLandingRepository = parachuteLandingRepository;
            _eventMatchStatusRepository = eventMatchStatusRepository;
            _deskPositionRepository = deskPositionRepository;
            _logger = logger;
        }

        public dynamic DeleteDocumentCollections(string matchId)
        {
            var matchRankingFilter = Builders<MatchRanking>.Filter.Eq(s => s.MatchId, matchId);

            var matchRankingResponse = _matchRankingRepository.DeleteMany(matchRankingFilter, "MatchRanking");

            var killFilter = Builders<Kill>.Filter.Eq(s => s.MatchId, matchId);

            var killResponse = _killRepostiory.DeleteMany(killFilter, "Kill");

            var liveEventKillFilter = Builders<LiveEventKill>.Filter.Empty;

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

            var matchSummaryFilter = Builders<MatchSummaryData>.Filter.Eq(s => s.Id, matchId);

            var matchSummaryResponse = _tournamentRepository.DeleteOne(tournamentMatchIdFilter, "MatchSummary");

            var parachuteLandingFilter = Builders<ParachuteLanding>.Filter.Eq(s => s.MatchId, matchId);

            var parachuteLandingResponse = _parachuteLandingRepository.DeleteMany(parachuteLandingFilter, "ParachuteLanding");
                
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
                    "CompletedSuccessfully".WithValue(tournamentMatchIdResponse.IsCompletedSuccessfully)),
                 new ExpandoObject().Init(
                    "CollectionName".WithValue("MatchSummary"),
                    "DocumentDeleteCount".WithValue(matchSummaryResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(matchSummaryResponse.IsCompletedSuccessfully)),
                 new ExpandoObject().Init(
                    "CollectionName".WithValue("ParachuteLanding"),
                    "DocumentDeleteCount".WithValue(parachuteLandingResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(parachuteLandingResponse.IsCompletedSuccessfully)
                     )
            };

            mongoDeletedCollection.Result = deletedColletionInfo;

            return mongoDeletedCollection;
        }

        public dynamic DeleteLiveDataDocument()
        {
            var liveEventMatchStatusFilter = Builders<EventLiveMatchStatus>.Filter.Empty;

            var liveEventMatchStatusResponse = _eventMatchStatusRepository.DeleteMany(liveEventMatchStatusFilter, "EventMatchStatus");

            var liveEventKillFilter = Builders<LiveEventKill>.Filter.Empty;

            var liveEventKillResponse = _liveEventKillRepository.DeleteMany(liveEventKillFilter, "LiveEventKill");

            var eventDamageFilter = Builders<EventDamage>.Filter.Empty;

            var eventDamageResponse = _eventDamageRepository.DeleteMany(eventDamageFilter, "LiveEventDamage");

            var liveMatchStatusFilter = Builders<LiveMatchStatus>.Filter.Empty;

            var liveMatchStatusResponse = _liveMatchStatusRepository.DeleteMany(liveMatchStatusFilter, "TeamLiveStatus");

            dynamic mongoDeletedCollection = new ExpandoObject();

            List<dynamic> deletedColletionInfo = new List<dynamic>() {
                new ExpandoObject().Init(
                    "CollectionName".WithValue("EventMatchStatus"),
                    "DocumentDeleteCount".WithValue(liveEventMatchStatusResponse.Result.DeletedCount),
                    "CompletedSuccessfully".WithValue(liveEventMatchStatusResponse.IsCompletedSuccessfully)),
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
                    "CompletedSuccessfully".WithValue(liveMatchStatusResponse.IsCompletedSuccessfully))
            };

            mongoDeletedCollection.Result = deletedColletionInfo;

            return mongoDeletedCollection;
        }

        public async Task<IEnumerable<Event>> GetMatchDetails()
        {
            var matchCollection = _tournamentRepository.GetMongoDbCollection("TournamentMatchId");

            var ranking = _matchRankingRepository.GetMongoDbCollection("MatchRanking");

            var matchRanking = ranking.FindAsync(Builders<MatchRanking>.Filter.Empty).Result.ToEnumerable().Select(se => new { Id = se.MatchId }).Distinct();

            var matchDetails = await matchCollection.FindAsync(Builders<Event>.Filter.Empty).Result.ToListAsync();

            var matchDetailsJoin = matchDetails.GroupJoin(matchRanking, left => left.Id, right => right.Id,
                                                            (left, right) => new { TableA = right, TableB = left }).SelectMany(p => p.TableA.DefaultIfEmpty(), (x, y) => new { TableA = y, TableB = x.TableB });

            var matchDetailStatusUpdate = new List<Event>();

            foreach (var item in matchDetailsJoin)
            {
                if (item.TableA != null && item.TableB != null)
                {
                    matchDetailStatusUpdate.Add(new Event()
                    {
                        Id = item.TableB.Id,
                        MatchId = item.TableB.MatchId,
                        CreatedAT = item.TableB.CreatedAT,
                        DataAvailable = true,
                        EventName = item.TableB.EventName
                    });
                }
                else
                {
                    matchDetailStatusUpdate.Add(new Event()
                    {
                        Id = item.TableB.Id,
                        MatchId = item.TableB.MatchId,
                        CreatedAT = item.TableB.CreatedAT,
                        DataAvailable = false,
                        EventName = item.TableB.EventName
                    });
                }

            }
            return matchDetailStatusUpdate;
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
                                                    new
                                                    {
                                                        Name = (string)m.SelectToken("id"),
                                                        CreatedDate = (string)m.SelectToken("attributes.createdAt")
                                                    }
                                                    ).ToList();

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

        public async Task<object> GetLiveLatestMatch(string tournamentName)
        {
            try
            {
                _logger.LogInformation("Live API Request Started" + Environment.NewLine);

                var tournamentsResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateLiveRequestHeader(), "tournaments/" + tournamentName + "/matches/most_recent");

                if (tournamentsResponse.Result.StatusCode == HttpStatusCode.OK && tournamentsResponse != null)
                {
                    _logger.LogInformation("Live API Response Json" + Environment.NewLine);

                    var response = tournamentsResponse.Result.Content.ReadAsStringAsync().Result;

                    var o = JObject.Parse(response);

                    var liveMatchId = (string)o.SelectToken("data.id");

                    return liveMatchId.Split(".").Last(); ;
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

        public void CreatePlayerDeskPosition(IEnumerable<DeskSeatings> seatingPosition)
        {
             var deskSeatings = seatingPosition.Select(s => new DeskSeatingPosition(){
                 DeskNumber = s.DeskNumber,
                 TeamIdShort = s.TeamIdShort,
                 Seat1PlayerId = s.Seat1PlayerId,
                 Seat2PlayerId = s.Seat2PlayerId,
                 Seat3PlayerId = s.Seat3PlayerId,
                 Seat4PlayerId = s.Seat4PlayerId
             });

            _deskPositionRepository.Insert(deskSeatings, "DeskSeatingPosition");
        }

        public void EditPlayerDeskPosition(IEnumerable<DeskSeatings> seatingPosition)
        {
            var playerSeatings = seatingPosition.Select(s => new DeskSeatingPosition()
            {
                DeskNumber = s.DeskNumber,
                TeamIdShort = s.TeamIdShort,
                Seat1PlayerId = s.Seat1PlayerId,
                Seat2PlayerId = s.Seat2PlayerId,
                Seat3PlayerId = s.Seat3PlayerId,
                Seat4PlayerId = s.Seat4PlayerId
            });

            var deskSeatings = _deskPositionRepository.GetMongoDbCollection("DeskSeatingPosition");

            

            foreach (var desk in playerSeatings)
            {
                var exitingSeatingPositions = deskSeatings.Find(Builders<DeskSeatingPosition>.Filter.Where(cn => cn.DeskNumber == desk.DeskNumber)).FirstOrDefault();

                var newSeatingPositions = new DeskSeatingPosition() {
                                    Id = exitingSeatingPositions.Id,
                                    DeskNumber = desk.DeskNumber,
                                    TeamIdShort = desk.TeamIdShort,
                                    Seat1PlayerId = desk.Seat1PlayerId,
                                    Seat2PlayerId = desk.Seat2PlayerId,
                                    Seat3PlayerId = desk.Seat3PlayerId,
                                    Seat4PlayerId = desk.Seat4PlayerId
                                   };

                var filter = Builders<DeskSeatingPosition>.Filter.Eq(s => s.Id, exitingSeatingPositions.Id);

                _deskPositionRepository.Replace(newSeatingPositions, filter, "DeskSeatingPosition");

            }


        }

        public async Task<IEnumerable<DeskSeatings>> GetPlayerDeskPositions()
        {
            var deskSeatings = _deskPositionRepository.GetMongoDbCollection("DeskSeatingPosition");

            var seatingPositions = deskSeatings.FindAsync(Builders<DeskSeatingPosition>.Filter.Empty).Result.ToEnumerable()
                                        .Select(s => new DeskSeatings() {
                                                        DeskNumber = s.DeskNumber,
                                                        TeamIdShort = s.TeamIdShort,
                                                        Seat1PlayerId = s.Seat1PlayerId,
                                                        Seat2PlayerId = s.Seat2PlayerId,
                                                        Seat3PlayerId = s.Seat3PlayerId,
                                                        Seat4PlayerId = s.Seat4PlayerId
                                              });



            return await Task.FromResult(seatingPositions);

        }
    }
}
