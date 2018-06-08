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

            var text = playerKilled.Select(s => s.Victim).GroupBy(g => g.TeamId);
            return null;
            //throw new NotImplementedException();
        }
    }
}
