﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface ILiveStats
    {
        Task<IEnumerable<LiveMatchStatus>> GetLiveStatus(int matchId);
        
        Task<IEnumerable<MatchRanking>> GetLiveStatsRanking(int matchId);
    }
}