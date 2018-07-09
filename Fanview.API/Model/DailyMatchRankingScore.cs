using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class DailyMatchRankingScore
    {
        public string MatchId { get; set; }
        public IEnumerable<MatchRanking> MatchRankingScores { get; set; }
    }
}
