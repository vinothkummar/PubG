﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IPlayerKilled
    {
        IEnumerable<string> GetPlayerKilledText(string matchID);
        IEnumerable<string> GetLast4PlayerKilledText (string matchID);
        Task<IEnumerable<KilliPrinter>> GetLivePlayerKilled();
    }
}
