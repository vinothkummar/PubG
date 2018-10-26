﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IPlayerKilled
    {
        IEnumerable<string> GetPlayerKilledText(string matchID);

        IEnumerable<string> GetLast4PlayerKilledText (string matchID);

        IEnumerable<KilliPrinter> GetPlayerKilled(string matchId);

        Task<IEnumerable<KilliPrinter>> GetLivePlayerKilled(int matchId);
    }
}
