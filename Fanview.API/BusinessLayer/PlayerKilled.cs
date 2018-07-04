using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer
{
    public class PlayerKilled : IPlayerKilled
    {
       
        List<IKillingRule> _rules = new List<IKillingRule>();
        private IPlayerKillRepository _playerKillRepository;
        private ITakeDamageRepository _takeDamageRepository;

        public PlayerKilled(IPlayerKillRepository playerKillRepository, ITakeDamageRepository takeDamageRepository)
        {
            _playerKillRepository = playerKillRepository;
            _takeDamageRepository = takeDamageRepository;

            _rules.Add(new IndividualPlayerKilled());
           
        }

        public IEnumerable<string> GetLast4PlayerKilledText(string matchId)
        {

            var playerKilledOrTeamEliminatedMessages = new List<string>();

            var kills =  _playerKillRepository.GetLast4PlayerKilled(matchId).Result;

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiation(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages.AddRange(output);
                }
            }


            return playerKilledOrTeamEliminatedMessages.OrderByDescending(o => o);
        }

        public IEnumerable<string> GetPlayerKilledText(string matchId)
        {
            var playerKilledOrTeamEliminatedMessages = new List<string>();

            var kills = _playerKillRepository.GetPlayerKilled(matchId).Result;            

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiation(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }


            return playerKilledOrTeamEliminatedMessages;
        }
    }
}
