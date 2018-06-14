using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Utility;

namespace Fanview.API.BusinessLayer
{
    public class PlayerKilled : IPlayerKilled
    {
        private ITelemetryRepository _telemetryApiRepository;
        List<IKillingRule> _rules = new List<IKillingRule>();

        public PlayerKilled(ITelemetryRepository telemetryApiRepository)
        {
            _telemetryApiRepository = telemetryApiRepository;

            _rules.Add(new IndividualPlayerKilled());
            _rules.Add(new TeamElimination());
        }

        public IEnumerable<string> GetPlayerKilled()
        {
            var resultFromDb = _telemetryApiRepository.GetPlayerKills().Result;

            var playerKilledOrTeamEliminatedMessages = new List<string>();

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiation(resultFromDb);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }
                

            return playerKilledOrTeamEliminatedMessages;
        }
    }
}
