using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class PlayerKillRepository : IPlayerKillRepository
    {
        private IGenericRepository<Kill> _Kill;
        private IGenericRepository<LiveEventKill> _LiveEventKill;
        private IGenericRepository<EventInfo> _eventInfoRepository;
        private IPlayerRepository _playerRepository;
        private IGenericRepository<CreatePlayer> _CreatePlayer;
        //private IGenericRepository<Event> _tournament;       
        private ILogger<PlayerKillRepository> _logger;
        private ITeamRepository _teamRepository;
        private IEventRepository _eventRepository;
        private IMemoryCache _cache;
        private List<LiveEventKill> _liveKilledCachedData;
        private Task<HttpResponseMessage> _pubGClientResponse;
        private DateTime killEventlastTimeStamp = DateTime.MinValue;
        private IClientBuilder _httpClientBuilder;
        private IHttpClientRequest _httpClientRequest;
        private IGenericRepository<MatchPlayerStats> _genericMatchPlayerStatsRepository;
        private IGenericRepository<TeamPlayer> _genericTeamPlayerRepository;
        private IGenericRepository<Team> _team;
        private IGenericRepository<TeamPlayer> _teamPlayers;
        private ITeamPlayerRepository _teamPlayerRepository;
        private IMatchRepository _matchRepository;


        private string _matchId;
       

        public PlayerKillRepository(IClientBuilder httpClientBuilder,
                                    IHttpClientRequest httpClientRequest,                                    
                                    IGenericRepository<Kill> genericRepository,
                                    IGenericRepository<CreatePlayer> genericPlayerRepository,
                                    IGenericRepository<LiveEventKill> genericLiveEventKillRepository,
                                    IGenericRepository<EventInfo> eventInfoRepository, 
                                    IGenericRepository<MatchPlayerStats> genericMatchPlayerStatsRepository,
                                    //IGenericRepository<Event> tournament,
                                    IGenericRepository<TeamPlayer> genericTeamPlayerRepository,
                                    IGenericRepository<Team> team,
                                    IGenericRepository<TeamPlayer> teamPlayers,
                                    IPlayerRepository playerRepository,
                                    ITeamPlayerRepository teamPlayerRepository,
                                    IMatchRepository matchRepository,
                                    ITeamRepository teamRepository,
                                    IEventRepository eventRepository,
                                    ILogger<PlayerKillRepository> logger,
                                    IMemoryCache cache)
        {
            _httpClientBuilder = httpClientBuilder;
            _httpClientRequest = httpClientRequest;  
            _Kill = genericRepository;
            _CreatePlayer = genericPlayerRepository;            
            _LiveEventKill = genericLiveEventKillRepository;
            _eventInfoRepository = eventInfoRepository;
            _playerRepository = playerRepository;
            _genericMatchPlayerStatsRepository = genericMatchPlayerStatsRepository;
            //_tournament = tournament;
            _genericTeamPlayerRepository = genericTeamPlayerRepository;
            _team = team;
            _teamPlayers = teamPlayers;
            _teamPlayerRepository = teamPlayerRepository;            
            _matchRepository = matchRepository;
            _logger = logger;
            _teamRepository = teamRepository;
            _eventRepository = eventRepository;
            _cache = cache;
            _liveKilledCachedData = new List<LiveEventKill>();
            

        }
        

        private IEnumerable<Kill> GetLogPlayerKilled(JArray jsonToJObject, string matchId)
        {
            var result =  jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerKill").Select(s => new Kill()
            {
                MatchId = matchId,
                AttackId = s.Value<int>("attackId"),
                Killer = new Killer()
                {
                    Name = (string)s["killer"]["name"],
                    TeamId = (int)s["killer"]["teamId"],
                    Health = (float)s["killer"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["killer"]["location"]["x"],
                        y = (float)s["killer"]["location"]["y"],
                        z = (float)s["killer"]["location"]["z"],
                    },
                    Ranking = (int)s["killer"]["ranking"],
                    AccountId = (string)s["killer"]["accountId"]
                },
                Victim = new Victim()
                {
                    Name = (string)s["victim"]["name"],
                    TeamId = (int)s["victim"]["teamId"],
                    Health = (float)s["victim"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["victim"]["location"]["x"],
                        y = (float)s["victim"]["location"]["y"],
                        z = (float)s["victim"]["location"]["z"],
                    },
                    Ranking = (int)s["victim"]["ranking"],
                    AccountId = (string)s["victim"]["accountId"]
                },
                DamageTypeCategory = (string)s["damageTypeCategory"],               
                DamageCauserName = (string)s["damageCauserName"],
                Distance = (float)s["distance"],
                Common = new Common()
                {
                   // MatchId = (string)s["common"]["matchId"],
                   // MapName = (string)s["common"]["mapName"],
                    IsGame = (float)s["common"]["isGame"]

                },
               // Version = (int)s["_V"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]

            });

            return result;
        }

        public async void InsertPlayerKillTelemetry(string jsonResult, string matchId)
        {              
            var jsonToJObject = JArray.Parse(jsonResult);
           

            var playerCreated = _CreatePlayer.GetMongoDbCollection("PlayerCreated");

            var isPlayerCreated = await playerCreated.FindAsync(Builders<CreatePlayer>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isPlayerCreated.Count() == 0 || isPlayerCreated == null){

                IEnumerable<CreatePlayer> logPlayerCreate = GetLogPlayerCreated(jsonToJObject, matchId);

                Func<Task> persistPlayerToMongo = async () => _CreatePlayer.Insert(logPlayerCreate, "PlayerCreated");

                await Task.Run(persistPlayerToMongo);
            }

            var kills = _Kill.GetMongoDbCollection("Kill");

            var isKillExists = await kills.FindAsync(Builders<Kill>.Filter.Where(cn => cn.MatchId == matchId)).Result.ToListAsync();

            if (isKillExists.Count() == 0 || isKillExists == null)
            {
                IEnumerable<Kill> logPlayerKill = GetLogPlayerKilled(jsonToJObject, matchId);

                Func<Task> persistDataToMongo = async () => _Kill.Insert(logPlayerKill, "Kill");

                await Task.Run(persistDataToMongo);
            }

            
        }

        private IEnumerable<CreatePlayer> GetLogPlayerCreated(JArray jsonToJObject, string matchId)
        {
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerCreate").Select(s => new CreatePlayer()
            {
                MatchId = matchId,                
                Name = (string)s["character"]["name"],
                TeamId = (string)s["character"]["teamId"],
                Health = (float)s["character"]["health"],
                Location = new Location()
                {
                    x = (float)s["character"]["location"]["x"],
                    y = (float)s["character"]["location"]["y"],
                    z = (float)s["character"]["location"]["z"],
                },
                AccountId = (string)s["character"]["accountId"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]
            });

            return result;
        }

        public async Task<IEnumerable<Kill>> GetPlayerKilled(string matchId)
        {
            _logger.LogInformation("GetPlayedKiller Repository call started" + Environment.NewLine);
            try
            {                

                var response = _Kill.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayedKiller Repository call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerKilled");

                throw;
            }
        }

        public async Task<IEnumerable<Kill>> GetPlayerKilled(int matchId)
        {
            _logger.LogInformation("GetPlayedKiller Repository call started" + Environment.NewLine);
            try
            {
                var tournamentMatchId = _eventRepository.GetTournamentMatchId(matchId).Result;

                var response = _Kill.GetAll("Kill").Result.Where(cn => cn.MatchId == tournamentMatchId);

                _logger.LogInformation("GetPlayedKiller Repository call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerKilled");

                throw;
            }            
        }


        public async Task<IEnumerable<LiveEventKill>> GetLiveKilled(int matchId)
        {
            IEnumerable<LiveEventKill> Obj;

            bool isExists = _cache.TryGetValue("LiveKilled", out Obj);

            if (!isExists)
            {
                _logger.LogInformation("GetLivePlayerKilled Repository call started" + Environment.NewLine);
                try
                {
                    var tournamentMatchId = _eventRepository.GetTournamentMatchId(matchId).Result;

                    var response = _LiveEventKill.GetAll("LiveEventKill").Result.Where(cn => cn.MatchId == tournamentMatchId);

                    _logger.LogInformation("GetLivePlayerKilled Repository call completed" + Environment.NewLine);

                    return await Task.FromResult(response);

                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "GetlivePlayerKilled");

                    throw;
                }

            }

            return Obj;
        }

        public async Task<IEnumerable<Kill>> GetLast4PlayerKilled(string matchId)
        { 
            var response = _Kill.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId).TakeLast(4);

            return await Task.FromResult(response);
        }

        public async void PollTelemetryPlayerKilled(string matchId)
        {
            try
            {
                _logger.LogInformation("Match Request Started" + Environment.NewLine);

                //_pubGClientResponse = Task.Run(async () => await _servicerRequest.GetAsync(await _httpClient.CreateRequestHeader(), query));

                _pubGClientResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(), "shards/pc-tournaments/matches/" + matchId);

                if (_pubGClientResponse.Result.StatusCode == HttpStatusCode.OK && _pubGClientResponse != null)
                {
                    _logger.LogInformation("Match Player Stats  Response Json" + Environment.NewLine);

                    var jsonResult = _pubGClientResponse.Result.Content.ReadAsStringAsync().Result;

                    var _telemetryResponse = _httpClientRequest.GetAsync(await _httpClientBuilder.CreateRequestHeader(GetTelemetryUrl(jsonResult)), string.Empty);

                    var telemetryJsonResult = _telemetryResponse.Result.Content.ReadAsStringAsync().Result;

                    await Task.Run(async () => InsertPlayerKillTelemetry(telemetryJsonResult, matchId));
                    await Task.Run(async () => _playerRepository.InsertLogPlayerPosition(telemetryJsonResult, matchId));
                    await Task.Run(async () => _playerRepository.InsertVehicleLeaveTelemetry(telemetryJsonResult, matchId));
                    await Task.Run(async () => _matchRepository.InsertMatchSafeZonePosition(telemetryJsonResult, matchId));

                    //await Task.Run(async () => InsertMatchPlayerStats(jsonResult));

                    //await Task.Run(async () => _takeDamageRepository.InsertTakeDamageTelemetry(jsonResult));

                    //InsertMatchSummary(jsonResult);

                    _logger.LogInformation("Completed Loading Match Player Stats  Response Json" + Environment.NewLine);
                }

                _logger.LogInformation("Match Player Stats  Request Completed" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string GetTelemetryUrl(string jsonResult)
        {
            var jsonToJObject = JObject.Parse(jsonResult);

            var match = jsonToJObject.SelectToken("data").ToObject<MatchSummary>();

            JArray playerResultsIncluded = (JArray)jsonToJObject["included"];

            var telemetryAssets = playerResultsIncluded.Where(x => x.Value<string>("type") == "asset");

            var telemetryUrl = (string)telemetryAssets.Select(s => s.SelectToken("attributes.URL")).ElementAtOrDefault(0);

            return telemetryUrl;
        }

        public async Task<IEnumerable<Kill>> GetPlayerKilled(string matchId1, string matchId2, string matchId3, string matchId4)
        {
            _logger.LogInformation("GetPlayedKilled Repository Function call started" + Environment.NewLine);
            try
            {
                var response = _Kill.GetAll("Kill").Result.Where(cn => cn.MatchId == matchId1 || cn.MatchId == matchId2 || cn.MatchId == matchId3 || cn.MatchId == matchId4);

                // var response = _genericRepository.GetMongoDbCollection("Kill").FindAsyn(new BsonDocument());

                _logger.LogInformation("GetPlayedKilled Repository Function call completed" + Environment.NewLine);

                return await Task.FromResult(response);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayedKilled");

                throw;
            }
        }

        private IEnumerable<Kills> GetMatchPlayerStatsForMatchId(int matchId, int topCount)
        {
            _logger.LogInformation("GetMatchPlayerStatsForMatchId Repository Function call started" + Environment.NewLine);
            var defaultCount = 10;
            var rowCount = topCount > 0 ? topCount : defaultCount;

            var tournamentMatchId = _eventRepository.GetTournamentMatchId(matchId).Result;

            var matchstats = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");

            var matchstat = matchstats.FindAsync(Builders<MatchPlayerStats>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync().Result;

            var sortByKill = matchstat.AsQueryable().OrderByDescending(o => o.stats.Kills).Take(rowCount);

            var teams = _team.GetAll("Team").Result;

            var teamPlayers = _teamPlayerRepository.GetTeamPlayers().Result;

            var killList = sortByKill.Join(teams, s => s.TeamId, t => t.Id, (s, t) => new { s, t })
                                     .Join(teamPlayers, kl => kl.s.stats.Name.Trim(), tp => tp.PlayerName.Trim(), (kl, pl) => new { kl, pl })
                                     .Select(klt => new Kills()
                                     {                                        
                                        kills = klt.kl.s.stats.Kills,
                                        teamId = klt.kl.t.TeamId,
                                        timeSurvived = klt.kl.s.stats.TimeSurvived,
                                        playerId = klt.pl.PlayerId,
                                        playerName=klt.kl.s.stats.Name
                                     });

            _logger.LogInformation("GetMatchPlayerStatsForMatchId Repository Function call completed" + Environment.NewLine);

            return killList;
        }

        public async Task<KillLeader> GetKillLeaderList(int matchId,int topCount)
        {
            _logger.LogInformation("GetKillLeaderList with matchId and top count Repository Function call started" + Environment.NewLine);
            IEnumerable<Kills> killList;
            if (topCount > 0){
                killList = GetMatchPlayerStatsForMatchId(matchId, topCount);
            }
            else{
                killList = GetMatchPlayerStatsForMatchId(matchId, 0);
            }
            var killLeader = new KillLeader()
            {
                matchId = matchId,
                killList = killList
            };
            _logger.LogInformation("GetKillLeaderList with matchId and top count Repository Function call completed" + Environment.NewLine);
            return await Task.FromResult(killLeader);
        }

        public async Task<KillLeader> GetKillLeaderList()
        {
            _logger.LogInformation("GetKillLeaderList Repository Function call started" + Environment.NewLine);
            var matchstats = _genericMatchPlayerStatsRepository.GetMongoDbCollection("MatchPlayerStats");
            var matchstat = matchstats.FindAsync(new BsonDocument()).Result.ToListAsync().Result;
            var sortByKill = matchstat.AsQueryable().OrderByDescending(o => o.stats.Kills);

            var teams = _team.GetAll("Team").Result.ToList();
            var teamPlayers = _teamPlayerRepository.GetTeamPlayers().Result;

            var killList = sortByKill.Join(teams, s => s.TeamId, t => t.Id, (s, t) => new { s, t })
                                     .Join(teamPlayers, kl => kl.s.stats.Name.Trim(), tp => tp.PlayerName.Trim(), (kl, pl) => new { kl, pl })
                                     .GroupBy(g => g.pl.PlayerName)
                                     .Select(klt => new Kills()
                                     {
                                         kills =  klt.Sum(a => a.kl.s.stats.Kills),
                                         teamId = klt.Select(a => a.kl.t.TeamId).ElementAtOrDefault(0),
                                         timeSurvived = klt.Sum(a => a.kl.s.stats.TimeSurvived),
                                         playerId = klt.Select(a => a.pl.PlayerId).ElementAtOrDefault(0),
                                         playerName = klt.Key,
                                         DamageDealt = klt.Sum(a => a.kl.s.stats.DamageDealt)
                                     }).OrderByDescending(o => o.kills);

            var killLeaders = new KillLeader()
            {
                matchId = 0,
                killList = killList
            };
            _logger.LogInformation("GetKillLeaderList Repository Function call completed" + Environment.NewLine);
            return killLeaders;
        }

        public async Task<KillLeader> GetLiveKillList(int matchId, int topCount)
        {
            var teams = _teamRepository.GetAllTeam().Result.AsQueryable();

            _logger.LogInformation("GetLiveKillList Repository Function call started" + Environment.NewLine);

            var tournamentMatchId = await _eventRepository.GetTournamentMatchId(matchId);

            var teamPlayers = _teamPlayerRepository.GetTeamPlayers().Result;

            var liveEventKillList = _LiveEventKill.GetAll("LiveEventKill").Result.Where(cn => cn.MatchId == tournamentMatchId)
                                                  .Join(teamPlayers, lek => lek.VictimTeamId, tp => tp.TeamIdShort, (lek, tp) => new { lek, tp })
                                                  .Join(teams, pktp => new { TeamShortId = pktp.tp.TeamIdShort }, t => new { TeamShortId = t.TeamId }, (pktp, t) => new { pktp, t })
                                                  .Where(cn => cn.pktp.lek.IsGroggy == false)
                                                  .GroupBy(g => g.pktp.lek.KillerName).Select(s => new Kills()
                                                  {
                kills = s.Count(),
                playerName = s.Key,
                playerId = s.Select(a => a.pktp.tp.PlayerId).ElementAtOrDefault(0),
                teamId = s.Select(a => a.pktp.lek.KillerTeamId).ElementAtOrDefault(0)
            }).OrderByDescending(o => o.kills).Take(topCount > 0 ? topCount : 10);           
                                                
            var killLeaders = new KillLeader()
            {
                matchId = matchId,
                killList = liveEventKillList
            };
            return killLeaders;
        }

        public Task<IEnumerable<KillZone>> GetKillZone(int matchId)
        {
            var playerKilledFromOpenApi = GetPlayerKilled(matchId).Result.Select(s => new KillZone() {

                MatchId = s.MatchId,
                PlayerName = s.Victim.Name,
                TeamId = s.Victim.TeamId,
                Health = s.Victim.Health,
                Location = s.Victim.Location
            });


            return Task.FromResult(playerKilledFromOpenApi);
        }

        public void InsertLiveKillEventTelemetry(JObject[] jsonResult, string fileName)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);            

            if (jsonResult.Where(cn => (string)cn["_T"] == "EventMatchJoin").Count() > 0)
            {
               _matchId = jsonResult.Where(cn => (string)cn["_T"] == "EventMatchJoin").Select(s => s.Value<string>("matchId")).FirstOrDefault();

                var matchJoinTime = jsonResult.Where(cn => (string)cn["_T"] == "EventMatchJoin").Select(s => s.Value<double>("time")).FirstOrDefault();
                
                if (_matchId != null)
                {
                    CreateMatch(dateTime, matchJoinTime);
                }
            }

            var kills = jsonResult.Where(cn => (string)cn["_T"] == "EventKill").Select(s => new LiveEventKill()
            {
                MatchId = _matchId,
                IsDetailStatus = s.Value<bool>("isDetailStatus"),
                IsKillerMe = s.Value<bool>("isKillerMe"),
                KillerName = s.Value<string>("killerName"),
                KillerLocation = new Location()
                {
                    x = (float)s["killerLocation"]["x"],
                    y = (float)s["killerLocation"]["y"],
                    z = (float)s["killerLocation"]["z"],
                },
                KillerTeamId = s.Value<int>("killerTeamId"),
                IsVictimMe = s.Value<bool>("isVictimMe"),
                VictimName = s.Value<string>("victimName"),
                VictimLocation = new Location()
                {
                    x = (float)s["victimLocation"]["x"],
                    y = (float)s["victimLocation"]["y"],
                    z = (float)s["victimLocation"]["z"],
                },
                VictimTeamId = s.Value<int>("victimTeamId"),
                DamageCauser = s.Value<string>("damageCauser"),
                DamageReason = s.Value<string>("damageReason"),
                IsGroggy = s.Value<bool>("isGroggy"),
                IsStealKilled = s.Value<bool>("isStealKilled"),
                Version = (int)s["_V"],
                EventType = (string)s["_T"],
                EventTimeStamp = dateTime.AddSeconds((double)s["time"]).ToString(),
                EventSourceFileName = fileName

            });

            if(kills.Count() > 0){

                foreach (var itemLiveKilled in kills)
                {
                    _liveKilledCachedData.Add(itemLiveKilled);

                    _cache.Set("LiveKilled", _liveKilledCachedData);
                }

                _LiveEventKill.Insert(kills.ToList(), "LiveEventKill");
            }

        }

        private void CreateMatch(DateTime dateTime, double matchJoinTime)
        {
            _matchId = _matchId.Split(".").ElementAtOrDefault(9);

            var tournamentMatch = _eventRepository.FindEvent(_matchId).Result;

            if (tournamentMatch == null)
            {
                var tournamentMatchIdCount = _eventRepository.GetTournamentMatchCount().Result;

                var tournamentMatchDetails = new Event()
                {
                    Id = _matchId,
                    EventName = "PUBG Global Invitational Berlin 2018",
                    MatchId = tournamentMatchIdCount + 1,
                    CreatedAT = dateTime.AddSeconds((double)matchJoinTime).ToString()
                };

                _eventRepository.CreateAnEvent(tournamentMatchDetails);

                _logger.LogInformation("Live Killing Match Id Is created in the Tournament Match Id" + Environment.NewLine);
            }
        }

        public async Task<IEnumerable<LiveKillCount>> GetLiveKillCount(IEnumerable<LiveEventKill> liveEventKills)
        {
            var killCount = liveEventKills.GroupBy(g => g.KillerName).Select(s => new LiveKillCount()
            {
                KillerName = s.Key,
                KillerTeamId = s.Select(a => a.KillerTeamId).ElementAtOrDefault(0),
                KillCount = s.Count()
            }).OrderByDescending(o => o.KillCount);

            return await Task.FromResult(killCount);
        }

        public Task<KillLeader> GetKillLeaderListTopByTimed()
        {
           var KillLeaderList = GetKillLeaderList();

            var KillLeaderListByTimeTopped = KillLeaderList.Result.killList.OrderByDescending(o => o.timeSurvived);

            var killLeaders = new KillLeader()
            {
                matchId = 0,
                killList = KillLeaderListByTimeTopped
            };

            return Task.FromResult(killLeaders);
        }

        public Task<KillLeader> GetKillLeaderListToppedByDamageDealt()
        {
            var KillLeaderList = GetKillLeaderList();

            var KillListToppedByDamageDealt = KillLeaderList.Result.killList.OrderByDescending(o => o.DamageDealt);

            var killLeaders = new KillLeader()
            {
                matchId = 0,
                killList = KillListToppedByDamageDealt
            };

            return Task.FromResult(killLeaders);
        }
    }
}
