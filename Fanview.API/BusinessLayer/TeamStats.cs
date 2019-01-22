using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.BusinessLayer
{
    public class TeamStats : ITeamStats
    {
        private IGenericRepository<Team> _team;
        private IGenericRepository<Kill> _kill;
        private IGenericRepository<MatchPlayerStats> _matchPlayerStats;
        private IGenericRepository<TeamPlayer> _teamPlayers;
        private IGenericRepository<Event> _tournament;
        private IGenericRepository<VehicleLeave> _vehicleLeave;
        private IGenericRepository<PlayerPoition> _teamPlayersPosition;
        private IMatchRepository _matchRepository;
        private IRanking _ranking;
        private ILogger<TeamStats> _logger;

        public TeamStats(     IGenericRepository<Team> team,
                              IGenericRepository<Kill> kill,
                              IGenericRepository<MatchPlayerStats> matchPlayerStats,
                              IGenericRepository<TeamPlayer> teamPlayers,
                              IGenericRepository<Event> tournament,
                              IGenericRepository<VehicleLeave> vehicleLeave,
                              IGenericRepository<PlayerPoition> teamPlayersPosition,
                              IMatchRepository matchRepository,
                              IRanking ranking,
                              ILogger<TeamStats> logger)
        {
            _team = team;
            _kill = kill;
            _matchPlayerStats = matchPlayerStats;
            _teamPlayers = teamPlayers;
            _tournament = tournament;
            _vehicleLeave = vehicleLeave;
            _teamPlayersPosition = teamPlayersPosition;
            _matchRepository = matchRepository;
            _ranking = ranking;
            _logger = logger;
        }

        public async Task<TeamRoute> GetTeamRoute(int matchId)
        {
            var teamsCollection = _team.GetMongoDbCollection("Team").AsQueryable();             

            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var teamStatsRanking = _ranking.GetMatchRankings(matchId).Result.Take(4);           

            var logPlayersPosition = _teamPlayersPosition.GetMongoDbCollection("PlayerPosition");
            
            var matchPlayerPosition = logPlayersPosition.FindAsync(Builders<PlayerPoition>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result
                                        .ToListAsync().Result.OrderBy(o => o.EventTimeStamp);

            var playerLocation = matchPlayerPosition.Join(teamStatsRanking, mpp => mpp.TeamId, t => t.TeamId, (mpp, t) => new { mpp, t })
                                                    .OrderBy(o => o.mpp.TeamId).ThenBy(o1 => o1.mpp.Name)
                                                    .Select(s => new
                                                    {
                                                        TeamName = s.t.TeamName,
                                                        TeamRank = s.t.TeamRank,
                                                        PlayerName = s.mpp.Name,
                                                        Health = s.mpp.Health,
                                                        s.mpp.NumAlivePlayers,
                                                        s.mpp.Location,
                                                        EventTimeStamp = s.mpp.EventTimeStamp.ToDateTimeFormat(),
                                                        Ranking = s.mpp.Ranking,
                                                        TeamId = s.mpp.TeamId,                                                      
                                                        FanviewTeamId = s.t.TeamId.ToString()
                                                    });

            

            var vehicleLanding = _vehicleLeave.GetMongoDbCollection("VehicleLeave");

            var playerVehicleLeave = vehicleLanding.FindAsync(Builders<VehicleLeave>.Filter.Where(cn => cn.MatchId == tournamentMatchId && cn.Vehicle.VehicleType == "Parachute"))
                                                     .Result.ToListAsync().Result.OrderByDescending(o => o.EventTimeStamp).GroupBy(g => g.Character.Name).Select(s => new {
                                                      MatchId = s.Select(a => a.MatchId).ElementAtOrDefault(0),
                                                      Character = s.Select(a => a.Character).ElementAtOrDefault(0),
                                                      Vehicle = s.Select(a => a.Vehicle).ElementAtOrDefault(0),
                                                      RideDistance = s.Select( a => a.RideDistance).ElementAtOrDefault(0),
                                                      seatIndex  = s.Select(a => a.seatIndex).ElementAtOrDefault(0),
                                                      Common = s.Select(a => a.Common).ElementAtOrDefault(0),
                                                      Version = s.Select(a => a.Version).ElementAtOrDefault(0),
                                                      EventTimeStamp = s.Select(a => a.EventTimeStamp).ElementAtOrDefault(0).ToDateTimeFormat(),
                                                      EventType = s.Select(a => a.EventType).ElementAtOrDefault(0)
                                                   }).OrderBy(o => o.EventTimeStamp);

                                                   

            var playerVehicleLeaveTop3Teams = playerVehicleLeave.Where(cn => teamStatsRanking.Select(s => s.TeamId).Contains(cn.Character.TeamId));

            var logPlayerKilled = _kill.GetMongoDbCollection("Kill").FindAsync(Builders<Kill>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync().Result
                                                                  .Join(playerVehicleLeaveTop3Teams, k => new { Name = k.Victim.Name }, t => new { Name = t.Character.Name },
                                                                      (k, t) => new { k }).Distinct().OrderByDescending(o => o.k.EventTimeStamp);

            var longestSurvivingTeamPlayers = playerLocation.Join(playerVehicleLeaveTop3Teams, pl => new { TeamId = pl.TeamId }, plvtop3 => new { TeamId = plvtop3.Character.TeamId }, (pl, plvtop3) => new { pl, plvtop3 })
                                                            .Where(cn => cn.pl.PlayerName.Trim() == cn.plvtop3.Character.Name.Trim())
                                                            .OrderByDescending(t => t.pl.EventTimeStamp)
                                                            .GroupBy(g => new { TeamId = g.pl.TeamId })
                                                            .Select(s => new{
                                                                TeamID = s.Select(a => a.pl.TeamId).ElementAtOrDefault(0),                                                              
                                                                TeamName = s.Select(a => a.pl.TeamName).ElementAtOrDefault(0),
                                                                TeamRank = s.Select(a => a.pl.TeamRank).ElementAtOrDefault(0),
                                                                PlayerName = s.Select(a => a.pl.PlayerName).ElementAtOrDefault(0),
                                                                EventTimeStamp = s.Select(a => a.plvtop3.EventTimeStamp).ElementAtOrDefault(0),
                                                                TeamRoute = s.Select(a => a.pl.Location)
                                                            });

            var longestSurvivingLocation = playerLocation.Join(longestSurvivingTeamPlayers, pl => new { TeamId = pl.TeamId, Name = pl.PlayerName }, plvtop3 => new { TeamId = plvtop3.TeamID, Name = plvtop3.PlayerName }, (pl, plvtop3) => new { pl, plvtop3 })
                                      .Where(cn => (cn.pl.EventTimeStamp.TimeOfDay > cn.plvtop3.EventTimeStamp.TimeOfDay))
                                      .OrderBy(o => o.pl.EventTimeStamp).GroupBy(g => g.pl.TeamId)
                                      .Select(s => new Route()
                                      {
                                          TeamId = s.Select(a => a.pl.TeamId).ElementAtOrDefault(0),                                         
                                          TeamName = s.Select(a => a.pl.TeamName).ElementAtOrDefault(0),
                                          TeamRank = s.Select(a => a.pl.TeamRank).ElementAtOrDefault(0),
                                          PlayerName = s.Select(a => a.pl.PlayerName).ElementAtOrDefault(0),
                                          TeamRoute = s.Select(a => a.pl.Location)
                                      });

            var teamRoute = new TeamRoute();

            teamRoute.MatchId = matchId;

            teamRoute.MapName = _matchRepository.GetMapName(tournamentMatchId).Result;


            var routes = new List<Route>();

            foreach (var item in longestSurvivingLocation)
            {
                var locationList = new List<Location>();

                foreach (var item1 in item.TeamRoute.Select(c => new { x = c.x, y = c.y, z = c.z }).Distinct())
                {

                    locationList.Add(new Location()
                    {
                        x = item1.x,
                        y = item1.y,
                        z = item1.z
                    });
                }

               
                var route = new Route();

                route.TeamId = item.TeamId;             
                route.TeamName = item.TeamName;
                route.TeamRank = item.TeamRank;
                route.PlayerName = item.PlayerName;
                route.TeamRoute = locationList;

                routes.Add(route);

                teamRoute.Route = routes.OrderBy(o => o.TeamRank).ToList();
            }

            return teamRoute;           
        }       
    }
}
