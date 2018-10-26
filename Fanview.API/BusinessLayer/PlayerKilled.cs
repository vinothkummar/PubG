using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Fanview.API.BusinessLayer
{
    public class PlayerKilled : IPlayerKilled
    {
       
        List<IKillingRule> _rules = new List<IKillingRule>();
        private IPlayerKillRepository _playerKillRepository;       
        private ILogger<PlayerKilled> _logger;
        private IReadAssets _readAssets;
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _playerRepository;
        private IEventRepository _eventRepository;

        public PlayerKilled(IPlayerKillRepository playerKillRepository,                           
                            ILogger<PlayerKilled> logger,
                            IReadAssets readAssets,
                            ITeamPlayerRepository playerRepository,
                            IEventRepository eventRepository,
                            ITeamRepository teamRepository)
        {
            _playerKillRepository = playerKillRepository;
            _logger = logger;
            _readAssets = readAssets;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            
            _eventRepository = eventRepository;

           // _rules.Add(new IndividualPlayerKilled(_readAssets, _teamRepository, playerRepository));
           
        }

        public IEnumerable<string> GetLast4PlayerKilledText(string matchId)
        {

            var playerKilledOrTeamEliminatedMessages = new List<string>();

            var kills =  _playerKillRepository.GetLast4PlayerKilled(matchId).Result;

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiatedText(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages.AddRange(output);
                }
            }


            return playerKilledOrTeamEliminatedMessages;
        }

        public IEnumerable<string> GetPlayerKilledText(string matchId)
        {
            _logger.LogInformation("GetPlayedKilledText Business Layer Function call started" + Environment.NewLine);

            try
            {
                var playerKilledOrTeamEliminatedMessages = new List<string>();

                var kills = _playerKillRepository.GetPlayerKilled(matchId).Result;

                foreach (var rule in _rules)
                {
                    var output = rule.PlayerKilledOrTeamEliminiatedText(kills);

                    if (output != null)
                    {
                        playerKilledOrTeamEliminatedMessages = output.ToList();
                    }
                }

                _logger.LogInformation("GetPlayedKilledText Business Layer Function call completed" + Environment.NewLine);

                return playerKilledOrTeamEliminatedMessages;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerKilledText");

                throw;
            }
           
        }

        public IEnumerable<KilliPrinter> GetPlayerKilled(string matchId)
        {

            var playerKilledOrTeamEliminatedMessages = new List<KilliPrinter>();

            var kills = _playerKillRepository.GetPlayerKilled(matchId).Result;
            

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiated(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }
            return playerKilledOrTeamEliminatedMessages;
        }

        public async Task<IEnumerable<KilliPrinter>> GetLivePlayerKilled(int matchId)
        {   
            var playerKilledOrTeamEliminatedMessages = new List<KilliPrinter>();

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result;

            var tournamentMatchCreatedAt =  _eventRepository.GetEventCreatedAt(matchId).Result;
        
            playerKilledOrTeamEliminatedMessages = LiveKilledOrTeamEliminiated(kills, tournamentMatchCreatedAt).ToList();

            //foreach (var rule in _rules)
            //{
            //    var output = rule.LiveKilledOrTeamEliminiated(kills, tournamentMatchCreatedAt);

            //    if (output != null)
            //    {
            //        playerKilledOrTeamEliminatedMessages = output.ToList();
            //    }
            //}

            return await Task.FromResult(playerKilledOrTeamEliminatedMessages);
        }

        private IEnumerable<KilliPrinter> LiveKilledOrTeamEliminiated(IEnumerable<LiveEventKill> playerKilled, string CreateAt)
        {
            var teamPlayers = _playerRepository.GetTeamPlayers().Result;


            var result = playerKilled.Join(teamPlayers, pk => pk.VictimName.Trim(), tp => tp.PlayerName.Trim(), (pk, tp) => new { pk, tp })
                                      .Select(s => new
                                      {
                                          TimeKilled = s.pk.EventTimeStamp,
                                          KillerName = s.pk.KillerName,
                                          VictimName = s.pk.VictimName,
                                          DamageCause = s.pk.DamageCauser,
                                          s.pk.DamageReason,
                                          VictimTeamId = s.pk.VictimTeamId,
                                          KillerTeamId = s.pk.KillerTeamId,
                                          s.pk.IsGroggy,
                                          VictimPlayerId = s.tp.PlayerId,
                                          // KillerPlayerId = s.PlayerId
                                      }).OrderBy(o => o.TimeKilled);

            var killiPrinter = new List<KilliPrinter>();



            var teamCount = new List<int>();

            foreach (var item in result.Where(c => c.IsGroggy == false))
            {
                var playerLeftCount = 79 - killiPrinter.Count();

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";





                var playerKillMessage = new PlayerKilledGraphics()
                {
                    TimeKilled = $"{item.TimeKilled}",
                    KillerName = $"{item.KillerName}",
                    FreeText1 = $"KILLED",
                    VictimName = $"{item.VictimName}",
                    FreeText2 = $"WITH",
                    DamagedCausedBy = $"{_readAssets.GetDamageCauserName(item.DamageCause)} ",
                    PlayerLeft = $"{playerLeft}",

                    VictimPlayerId = item.VictimPlayerId,
                };

                if (string.IsNullOrWhiteSpace(playerKillMessage.DamagedCausedBy))
                {
                    playerKillMessage.FreeText2 = string.Empty;
                }

                TeamEliminated teamEliminatedMessage = null;

                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playerKilled.Where(cn => cn.VictimTeamId == item.VictimTeamId && cn.IsGroggy == false).Count();

                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamEliminatedCount = killiPrinter.Where(cn => cn.TeamEliminated != null).Count() == 0 ? 1 : killiPrinter.Where(cn => cn.TeamEliminated != null).Count();

                    var teamLeftCount = playerKilled.GroupBy(g => g.KillerTeamId).Count() - teamEliminatedCount;

                    var teamLeft = teamLeftCount == 1 ? "winner" : teamLeftCount.ToString() + " LEFT";

                    teamEliminatedMessage = new TeamEliminated()
                    {
                        TimeElimnated = $"{item.TimeKilled}",
                        FreeText1 = $"Team",
                        TeamId = $"{item.VictimTeamId}",
                        FreeText2 = $"HAS BEEN ELIMINATED",
                        TeamLeft = $"{teamLeft}"
                    };
                }

                var killMessage = new KilliPrinter() { PlayerKilled = playerKillMessage, TeamEliminated = teamEliminatedMessage };


                killiPrinter.Add(killMessage);
            }

            return killiPrinter;
        }
    }
}
