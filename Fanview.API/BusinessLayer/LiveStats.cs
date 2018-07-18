using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer
{
    public class LiveStats : ILiveStats
    {
        private ILogger<LiveStats> _logger;
        private IPlayerKillRepository _playerKillRepository;
        private ITeamPlayerRepository _teamPlayerRepository;
        private ITeamRepository _teamRepository;
        private IGenericRepository<Event> _EventRepository;

        public LiveStats(IPlayerKillRepository playerKillRepository, ITeamPlayerRepository teamPlayerRepository,
                              ITeamRepository teamRepository,
                              IGenericRepository<Event> eventRepository,
                              ILogger<LiveStats> logger)
        {
            _logger = logger;
            _playerKillRepository = playerKillRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _teamRepository = teamRepository;
            _EventRepository = eventRepository;
        }

        public async Task<LiveStatus> GetLiveStatus(string matchId)
        {

           

            var teams = _teamRepository.GetAllTeam().Result.AsQueryable();
            

            var teamPlayer = _teamPlayerRepository.GetTeamPlayers().Result.AsQueryable();
           

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result.OrderBy(o => o.EventTimeStamp).AsQueryable();
            

            var playerKilled = kills.Join(teamPlayer, pk => new { VictimTeamId = pk.VictimTeamId }, tp => new { VictimTeamId = tp.TeamIdShort }, (pk, tp) => new { pk, tp })
                                    .Join(teams, pktp => new { TeamShortId = pktp.tp.TeamIdShort }, t => new { TeamShortId = t.TeamId }, (pktp, t) => new { pktp, t })
                                    .Select(s =>
                                        new LiveTeam()
                                        {
                                            Id = s.t.TeamId,
                                            Name = s.t.Name,
                                            TeamPlayers = new LiveTeamPlayers()
                                            {
                                                PlayerName = s.pktp.tp.PlayerName,
                                                PlayeId = s.pktp.tp.PubgAccountId,
                                                PlayerStatus = s.pktp.pk.IsGroggy,
                                                PlayerTeamId = s.pktp.pk.VictimTeamId,
                                                TimeKilled = s.pktp.pk.EventTimeStamp,
                                                location = new LiveLocation()
                                                {
                                                    X = s.pktp.pk.VictimLocation.x,
                                                    Y = s.pktp.pk.VictimLocation.y,
                                                    Z = s.pktp.pk.VictimLocation.z
                                                }
                                            }
                                        }).GroupBy(g => new { PlayerName = g.TeamPlayers.PlayerName, TeamId = g.Id, TeamName = g.Name });


            _logger.LogInformation("PlayerKilled " + playerKilled.Count() + Environment.NewLine);

            var liveStatsStatus = new List<LiveTeamPlayerStatus>();

            foreach (var item in playerKilled)
            {
                var teamPlayers = new List<LiveTeamPlayers>();

                foreach (var item1 in item)
                {
                    teamPlayers.Add(
                        new LiveTeamPlayers()
                        {
                            PlayeId = item1.TeamPlayers.PlayeId,
                            PlayerName = item1.TeamPlayers.PlayerName,
                            PlayerTeamId = item1.TeamPlayers.PlayerTeamId,
                            PlayerStatus = item1.TeamPlayers.PlayerStatus,
                            location = item1.TeamPlayers.location,
                            TimeKilled = item1.TeamPlayers.TimeKilled
                        });
                }

                var teamstats = new LiveTeamPlayerStatus();

                if (liveStatsStatus.Where(cn => cn.Id == item.Key.TeamId).Count() == 0)
                {
                    teamstats.Id = item.Key.TeamId;
                    teamstats.Name = item.Key.TeamName;
                    teamstats.TeamPlayers = teamPlayers.OrderByDescending(o => o.TimeKilled).TakeLast(4);

                    liveStatsStatus.Add(teamstats);
                }
            }

            var liveStatus = new LiveStatus();
            liveStatus.MatchID = matchId;
            liveStatus.teams = liveStatsStatus;

            return await Task.FromResult(liveStatus);
        }
    }
}
