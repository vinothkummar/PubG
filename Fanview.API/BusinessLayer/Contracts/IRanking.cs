using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IRanking
    {
        Task<IEnumerable<MatchRanking>> GetMatchRanking(string matchId);

        Task<IEnumerable<DailyMatchRankingScore>> GetSummaryRanking(string matchId1, string matchId2, string matchId3, string matchId4 );

        Task<IEnumerable<MatchRanking>> PollAndGetMatchRanking(string matchId);
    }
}
