using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer
{
    public class TeamElimination : IKillingRule
    {
        public IEnumerable<string> PlayerKilledOrTeamEliminiation(IEnumerable<PlayerKill> playerKilled)
        {

            var  groupByTeam = playerKilled.Select(s => new Victim() { AccountId= s.Victim.AccountId, Name = s.Victim.Name, TeamId =s.Victim.TeamId , Health = s.Victim.Health}).GroupBy(g => g.TeamId);

            var teamElimination = new List<string>();

            foreach (var item in groupByTeam)
            {
                var eliminationText = item;
            }

            return null;
            //throw new NotImplementedException();
        }
    }
}
