using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IKillingRule
    {
        IEnumerable<string> PlayerKilledOrTeamEliminiatedText(IEnumerable<Kill> playerKilled);
        Task<IEnumerable<KilliPrinter>> LiveKilledOrTeamEliminiated(IEnumerable<LiveEventKill> playerKilled);
    }
}
