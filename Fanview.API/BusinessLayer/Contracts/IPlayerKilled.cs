using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IPlayerKilled
    {
        IEnumerable<string> GetPlayerKilled();

        IEnumerable<string> GetLast4PlayerKilled();
    }
}
