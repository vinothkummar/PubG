using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using Fanview.API.Services.Interface;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Fanview.API.Utility;
using MongoDB.Driver;

namespace Fanview.API.Repository
{
    public class MatchSummaryRepository : IMatchSummaryRepository
    {
        private IClientBuilder _httpClientBuilder;        
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<MatchSummary> _genericMatchSummaryRepository;
        private IGenericRepository<MatchPlayerStats> _genericMatchPlayerStatsRepository;
        private IGenericRepository<TeamPlayer> _genericTeamPlayerRepository;
        private IGenericRepository<MatchRanking> _genericMatchRankingRepository;
        private IGenericRepository<LiveMatchStatus> _genericLiveMatchStatusRepository;
        private ITeamRepository _teamRepository;
        private IGenericRepository<Event> _tournament;
        private ITeamPlayerRepository _teamPlayerRepository;
        private IPlayerKillRepository _playerKillRepository;
        private ITeamLiveStatusRepository _teamLiveStatusRepository;
        private ILogger<PlayerKillRepository> _logger;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime LastMatchCreatedTimeStamp = DateTime.MinValue;

        public MatchSummaryRepository(IClientBuilder httpClientBuilder,
                                      IHttpClientRequest httpClientRequest,
                                      IGenericRepository<MatchSummary> genericMatchSummaryRepository,
                                      IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
                                      IGenericRepository<TeamPlayer> genericTeamPlayerRepository,
                                      IGenericRepository<MatchRanking> genericMatchRankingRepository,
                                      IGenericRepository<LiveMatchStatus> genericLiveMatchStatusRepository,
                                      IGenericRepository<Event> tournament,
                                      ITeamRepository teamRepository,
                                      ITeamPlayerRepository teamPlayerRepository,
                                      IPlayerKillRepository playerKillRepository,
                                      ITeamLiveStatusRepository teamLiveStatusRepository,
                                      ILogger<PlayerKillRepository> logger)
        {
            _httpClientBuilder = httpClientBuilder;            
            _httpClientRequest = httpClientRequest;
            _genericMatchSummaryRepository = genericMatchSummaryRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            _genericTeamPlayerRepository = genericTeamPlayerRepository;
            _genericMatchRankingRepository = genericMatchRankingRepository;
            _genericLiveMatchStatusRepository = genericLiveMatchStatusRepository;
            _teamRepository = teamRepository;
            _tournament = tournament;
            _teamPlayerRepository = teamPlayerRepository;
            _playerKillRepository = playerKillRepository;
            _teamLiveStatusRepository = teamLiveStatusRepository;
            _logger = logger;
        }

        private MatchSummary GetMatchSummaryData(JObject jsonToJObject)
        {
            var result = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var participantMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "participant");

            result.MatchParticipant = participantMatchPlayerStats.Select(s => new MatchParticipant()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                ParticipantAttributes = s.SelectToken("attributes").ToObject<ParticipantAttributes>()
            });

            var rosterMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "roster");

            result.MatchRoster = rosterMatchPlayerStats.Select(s => new MatchRoster()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                RosterAttributes = s.SelectToken("attributes").ToObject<RosterAttributes>(),
                RosterRelationShips = new RosterRelationShips()
                {
                    Participant = s.SelectToken("relationships.participants.data").Select(p =>  new Participant()
                    {
                        Id = (string)p["id"],
                        Type = (string)p["type"]
                    })
                }
            });
            
            return result;
        }

        public async void InsertMatchSummary(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var CurrentMatchCreatedTimeStamp = (string)jsonToJObject["data"]["attributes"]["createdAt"];

            var matchSummaryData = GetMatchSummaryData(jsonToJObject);

            if (CurrentMatchCreatedTimeStamp.ToDateTimeFormat()  >  LastMatchCreatedTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericMatchSummaryRepository.Insert(matchSummaryData, "MatchSummary");

                 await Task.Run(persistDataToMongo);

                LastMatchCreatedTimeStamp = CurrentMatchCreatedTimeStamp.ToDateTimeFormat();
            }
        }


        public async void PollMatchSummary(string matchId)
        {   
            try
            {
                _logger.LogInformation("Poll Match Summary Request Started" + Environment.NewLine);
                 
                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Poll Match Summary Requested Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                     await Task.Run(async () => InsertMatchSummary(jsonResult));

                    _logger.LogInformation("Completed Loading Poll Match Summary Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Poll Match Summary Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async void PollMatchParticipantStats(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Player Stats Request Started" + Environment.NewLine);

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Match Player Stats  Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () => InsertMatchPlayerStats(jsonResult, matchId));

                    _logger.LogInformation("Completed Loading Match Player Stats  Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Match Player Stats  Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async void InsertMatchPlayerStats(string jsonResult, string matchId)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var matchPlayerStatsCollection = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");

            var isMatchStatsExists = await matchPlayerStatsCollection.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isMatchStatsExists.Count()  == 0 || isMatchStatsExists == null )
            {
                var matchPlayerStats = GetMatchPlayerStas(jsonToJObject, matchId);

                Func<Task> persistDataToMongo = async () => _genericMatchPlayerStatsRepository.Insert(matchPlayerStats, "MatchPlayerStats");

                await Task.Run(persistDataToMongo);
            }
        }
        private IEnumerable<MatchPlayerStats> GetMatchPlayerStas(JObject jsonToJObject, string matchId)
        {
            var match = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var rosterMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "roster");

            var matchRoster = rosterMatchPlayerStats.Select(s => new MatchRoster()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                RosterAttributes = s.SelectToken("attributes").ToObject<RosterAttributes>(),
                RosterRelationShips = new RosterRelationShips()
                {
                    Participant = s.SelectToken("relationships.participants.data").Select(p => new Participant()
                    {
                        Id = (string)p["id"],
                        Type = (string)p["type"]
                    })
                }
            });
           
            var participantMatchPlayerStats = playerResultsIncluded.Where(x => x.Value<string>("type") == "participant");

            var matchParticipant = participantMatchPlayerStats.Select(s => new MatchParticipant()
            {
                Id = (string)s["id"],
                Type = (string)s["type"],
                ParticipantAttributes = s.SelectToken("attributes").ToObject<ParticipantAttributes>()
            });

            var teamPlayers = _teamPlayerRepository.GetTeamPlayers();

            var teamParticipants = new List<MatchPlayerStats>();

            var rosterTeamId = 0;

            foreach (var item in matchRoster)
            {
                rosterTeamId = item.RosterAttributes.Stats.TeamId;

                foreach (var item1 in item.RosterRelationShips.Participant )
                {
                    foreach (var item2 in matchParticipant)
                    {                        
                            var teamParticipant = new MatchPlayerStats();

                            if (item1.Id == item2.Id)
                            {
                                teamParticipant.MatchId = match.Id;
                                teamParticipant.RosterId = item.Id;
                                teamParticipant.ParticipantId = item2.Id;                              
                                teamParticipant.stats = item2.ParticipantAttributes.stats;
                                teamParticipant.TeamId = teamPlayers.Result.Where(cn => cn.TeamIdShort == rosterTeamId).FirstOrDefault().TeamId;
                                teamParticipant.Rank = item.RosterAttributes.Stats.Rank;
                                teamParticipant.ShortTeamId = item.RosterAttributes.Stats.TeamId;
                                teamParticipants.Add(teamParticipant);
                                
                            }
                    }
                }
            }
            return teamParticipants;
        }

        public void CreateAndMapTestTeamPlayerFromMatchHistory(string matchId)
        {
            var teamPlayers = new List<TeamPlayer>();

            var matchPlayerStatus = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result;

            foreach (var item1 in matchPlayerStatus.Where(cn => cn.MatchId == matchId).GroupBy(g => g.TeamId))
            {
                foreach (var item2 in item1)
                {
                    var teamPlayer = new TeamPlayer();

                    teamPlayer.MatchId = item2.MatchId;
                    teamPlayer.PlayerName = item2.stats.Name;
                    teamPlayer.PubgAccountId = item2.stats.PlayerId;
                    teamPlayer.TeamId = item2.TeamId;

                    teamPlayers.Add(teamPlayer);
                }
            }

            _genericTeamPlayerRepository.Insert(teamPlayers, "TeamPlayers");

        }

        public async Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId)
        {
            _logger.LogInformation("GetPlayerMatchStats Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result.Where(cn => cn.MatchId == matchId);

                _logger.LogInformation("GetPlayerMatchStats Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerMatchStats");

                throw;
            }
        }

        public async Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            _logger.LogInformation("GetPlayerMatchStats Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _genericMatchPlayerStatsRepository.GetAll("MatchPlayerStats").Result.Where(cn => cn.MatchId == matchId1 || cn.MatchId == matchId2 || cn.MatchId == matchId3 || cn.MatchId == matchId4);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayerMatchStats Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerMatchStats");

                throw;
            }
        }

        public async Task PollMatchRoundRankingData(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Round Ranking Data Request Started" + Environment.NewLine);               

                await Task.Run(async () => PollMatchParticipantStats(matchId));

                await Task.Run(async () => _playerKillRepository.PollTelemetryPlayerKilled(matchId));

                _logger.LogInformation("Match Round Ranking Data Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void InsertLiveEventMatchStatusTelemetry(JObject[] jsonResult, string fileName)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            
            var matchStatus = jsonResult.Where(x => x.Value<string>("_T") == "EventMatchStatus").Select(s => new EventLiveMatchStatus()
            {

                IsDetailStatus = (bool)s["isDetailStatus"],
                MatchId = s["matchId"].ToString().Split('.').Last(),
                TeamMode = (string)s["teamMode"],
                CameraMode = (string)s["camerMode"],
                MatchState = (string)s["matchState"],
                ElapsedTime = (int)s["elapsedTime"],
                BlueZonePhase = (int)s["blueZonePhase"],
                IsBlueZoneMoving = (bool)s["isBlueZoneMoving"],
                BlueZoneRadius = (int)s["blueZoneRadius"],
                BlueZoneLocation = new Location()
                {
                    x = (float)s["blueZoneLocation"]["x"],
                    y = (float)s["blueZoneLocation"]["y"],
                    z = (float)s["blueZoneLocation"]["z"],
                },
                WhiteZoneRadius = (int)s["whiteZoneRadius"],
                WhiteZoneLocation = new Location()
                {
                    x = (float)s["whiteZoneLocation"]["x"],
                    y = (float)s["whiteZoneLocation"]["y"],
                    z = (float)s["whiteZoneLocation"]["z"],
                },
                RedZoneRadius = (int)s["redZoneRadius"],
                RedZoneLocation = new Location()
                {
                    x = (float)s["redZoneLocation"]["x"],
                    y = (float)s["redZoneLocation"]["y"],
                    z = (float)s["redZoneLocation"]["z"],
                },
                StartPlayerCount = (int)s["startPlayerCount"],
                AlivePlayerCount = (int)s["alivePlayerCount"],
                StartTeamCount = (int)s["startTeamCount"],
                AliveTeamCount = (int)s["aliveTeamCount"],
                PlayerInfos = s["playerInfos"].Select(s1 => new EventMatchStatusPlayerInfo()
                {
                    PlayerName = (string)s1["playerName"],
                    TeamId = (int)s1["teamId"],
                    Location = new Location()
                    {
                        x = (float)s1["location"]["x"],
                        y = (float)s1["location"]["y"],
                        z = (float)s1["location"]["z"],
                    },
                    Health = (int)s1["health"],
                    BoostGauge = (int)s1["boostGauge"],
                    State = (string)s1["state"],
                    ArmedWeapon = (string)s1["armedWeapon"],
                    ArmedAmmoCount = (int)s1["armedAmmoCount"],
                    InventoryAmmoCount = (int)s1["inventoryAmmoCount"]
                }).ToList(),

                Version = (int)s["_V"],
                EventTimeStamp = (double)s["time"],
                EventType = (string)s["_T"],
                EventSourceFileName = fileName

            });

            if (matchStatus.Count() > 0)
            {
                _teamLiveStatusRepository.CreateEventLiveMatchStatus(matchStatus);

                var test = CreateMatchLiveStatus(matchStatus, matchStatus.Select(a => a.MatchId).ElementAtOrDefault(0));
            }

        }

        private async Task CreateMatchLiveStatus(IEnumerable<EventLiveMatchStatus> matchStatus, string matchId)
        {
            //var teamPlayerCollection = _genericTeamPlayerRepository.GetMongoDbCollection("TeamPlayers");

            //var teamPlayers = await teamPlayerCollection.FindAsync(Builders<TeamPlayer>.Filter.Empty).Result.ToListAsync();

            var teamPlayers = await _teamPlayerRepository.GetTeamPlayers();

            var liveMatchStatus = _genericLiveMatchStatusRepository.GetMongoDbCollection("TeamLiveStatus");

            //var isTeamLiveStatusCount = liveMatchStatus.FindAsync(Builders<LiveMatchStatus>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync().Result.Count;

            var isTeamLiveStatusCount = _teamLiveStatusRepository.GetTeamLiveStatusCount(matchId).Result;

            var matchPlayerStatus = matchStatus.Select(a => a.PlayerInfos.GroupBy(g => g.TeamId).OrderBy(o => o.Key));

            var matchStatusTimeStamp = matchStatus.Select(a => a.EventTimeStamp);

            var matchStatusMatchId = matchStatus.Select(a => a.MatchId);

            var teamLiveStatusCollection = new List<LiveMatchStatus>();

            foreach (var item in matchPlayerStatus)
            {
                foreach (var item1 in item)
                {
                    var teamLiveStatus = new LiveMatchStatus();
                    var teamId = item1.Select(s => s.TeamId).ElementAtOrDefault(0);
                    teamLiveStatus.TeamId = teamId;
                    teamLiveStatus.TeamName = _teamRepository.GetTeam().Result.Where(cn => cn.TeamId == teamId).Select(s => s.ShortName).ElementAtOrDefault(0);
                    var teamPlayerLiveStatusCollection = new List<LiveMatchPlayerStatus>();

                    int aliveCountIncremental = 0;
                    int deadCountIncremental = 0;

                    int aliveCount = 0;
                    int deadCount = 0;

                    foreach (var item2 in item1)
                    {
                        var teamPlayerStatus = new LiveMatchPlayerStatus();

                        teamPlayerStatus.PlayerId = teamPlayers.Where(cn => cn.PlayerName == item2.PlayerName).Select(a => a.PlayerId).FirstOrDefault();
                        teamPlayerStatus.PlayerName = item2.PlayerName;
                        teamPlayerStatus.Location = item2.Location;
                        teamPlayerStatus.Health = item2.Health;
                        teamPlayerStatus.BoostGauge = item2.BoostGauge;
                        teamPlayerStatus.State = item2.State;
                        teamPlayerStatus.ArmedWeapon = item2.ArmedWeapon;
                        teamPlayerStatus.ArmedAmmoCount = item2.ArmedAmmoCount;
                        teamPlayerStatus.InventoryAmmoCount = item2.InventoryAmmoCount;

                        teamPlayerStatus.IsAlive = item2.Health > 0 ? true : false;

                        aliveCount = item2.Health > 0 ? ++aliveCountIncremental : aliveCountIncremental;

                        deadCount = item2.Health > 0 ? deadCountIncremental : ++deadCountIncremental;

                        teamPlayerLiveStatusCollection.Add(teamPlayerStatus);

                        teamLiveStatus.TeamPlayers = teamPlayerLiveStatusCollection;

                        teamLiveStatus.AliveCount = aliveCount;

                        teamLiveStatus.DeadCount = deadCount;

                        teamLiveStatus.IsEliminated = deadCount == 4 ? true : false;

                        teamLiveStatus.MatchId = matchStatusMatchId.ElementAtOrDefault(0);

                        if (isTeamLiveStatusCount != 0 && teamLiveStatus.TeamId != 0)
                        {
                            var isTeamPlayerStatus = liveMatchStatus.FindAsync(Builders<LiveMatchStatus>.Filter.Where(cn => cn.TeamId == teamLiveStatus.TeamId && cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result;

                            if (isTeamPlayerStatus != null)
                            {
                                if (isTeamPlayerStatus.EliminatedAt == 0  && teamLiveStatus.IsEliminated == true)
                                {
                                    teamLiveStatus.EliminatedAt = matchStatusTimeStamp.ElementAtOrDefault(0);
                                }
                            }
                        }
                    }
                    teamLiveStatusCollection.Add(teamLiveStatus);
                }

                if (isTeamLiveStatusCount == 0)
                {
                    //_genericLiveMatchStatusRepository.Insert(teamLiveStatusCollection, "TeamLiveStatus");
                    _teamLiveStatusRepository.CreateTeamLiveStatus(teamLiveStatusCollection);
                }
                else
                {
                    foreach (var team in teamLiveStatusCollection)
                    {
                        if (team.TeamId != 0)
                        {
                            var document = liveMatchStatus.Find(Builders<LiveMatchStatus>.Filter.Where(cn => cn.TeamId == team.TeamId && cn.MatchId == matchId)).FirstOrDefault();

                            if (document.IsEliminated == false)
                            {

                                team.Id = document.Id;

                                var filter = Builders<LiveMatchStatus>.Filter.Eq(s => s.Id, document.Id);

                                _teamLiveStatusRepository.ReplaceTeamLiveStatus(team, filter);

                               //_genericLiveMatchStatusRepository.Replace(team, filter, "TeamLiveStatus");
                            }
                        }
                    }
                }
            }
        }

        public async Task<IEnumerable<LiveMatchStatus>> GetLiveMatchStatus(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var matchStatus = _genericLiveMatchStatusRepository.GetMongoDbCollection("TeamLiveStatus");

            var teamStatus = matchStatus.FindAsync(Builders<LiveMatchStatus>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync();

            return await teamStatus;
        }
    }
}
