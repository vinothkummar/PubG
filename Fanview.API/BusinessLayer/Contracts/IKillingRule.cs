using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IKillingRule
    {
        IEnumerable<string> PlayerKilledOrTeamEliminiation(IEnumerable<Kill> playerKilled);
    }
}
