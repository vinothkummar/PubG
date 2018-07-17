using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using Microsoft.Extensions.Logging;

namespace Fanview.API.BusinessLayer
{
    public class PlayerKilled : IPlayerKilled
    {
       
        List<IKillingRule> _rules = new List<IKillingRule>();
        private IPlayerKillRepository _playerKillRepository;       
        private ILogger<PlayerKilled> _logger;
        private IReadAssets _readAssets;
        private ITeamRepository _teamRepository;

        public PlayerKilled(IPlayerKillRepository playerKillRepository,                           
                            ILogger<PlayerKilled> logger,
                            IReadAssets readAssets,
                            ITeamRepository teamRepository)
        {
            _playerKillRepository = playerKillRepository;
            _logger = logger;
            _readAssets = readAssets;
            _teamRepository = teamRepository;

            _rules.Add(new IndividualPlayerKilled(_readAssets, _teamRepository));
           
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

        public IEnumerable<KilliPrinter> GetLivePlayerKilled(string matchId)
        {
            var playerKilledOrTeamEliminatedMessages = new List<KilliPrinter>();

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result;


            foreach (var rule in _rules)
            {
                var output = rule.LiveKilledOrTeamEliminiated(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }
            return playerKilledOrTeamEliminatedMessages;
        }
    }
}
