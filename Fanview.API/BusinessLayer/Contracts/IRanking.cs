using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.BusinessLayer.Contracts
{
    public interface IRanking
    {
        Task<IEnumerable<MatchRanking>> CalculateMatchRanking(string matchId);

        Task<IEnumerable<RankingResults>> GetMatchRankings(int matchId);

        //Task<IEnumerable<DailyMatchRankingScore>> GetSummaryRanking(string matchId1, string matchId2, string matchId3, string matchId4 );

        Task<IEnumerable<MatchRanking>> PollAndGetMatchRanking(string matchId);

        Task<IEnumerable<TournamentRanking>> GetTournamentRankings();

        Task<IEnumerable<RankingResults>> GetTournamentRankingByDay(int day);
    }
}
