using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private IRanking _ranking;

        public RankingController(IRanking ranking)
        {
            _ranking = ranking;
        }


        /// <summary>
        /// Returns match ranking order for the given the match Id.          
        /// </summary>
        /// <remarks>
        /// Sample request: MatchRanking/{matchId}         
        /// Calculate the Ranking Points stats for the team eliminated position.
        /// Calculate the Killing Points stats for the Player Killed.
        /// Calculate the cumulative total points for the single Match.
        /// Calculate the Team Ranking Order from the stats.
        /// </remarks>
        /// <param name='matchId'>1</param>
        [HttpGet("MatchRank/{matchId}", Name = "GetMatchRanking")]
        public async Task<IEnumerable<RankingResults>> GetMatchRanking(int matchId)
        {
            return await _ranking.GetMatchRankings(matchId);
            
        }

        ///// <summary>
        ///// Returns match Daily Summary Rankings for the given the match Id's         
        ///// </summary>
        ///// <remarks>
        ///// Sample request: DailyMatchSummaryRanking/{matchId}
        ///// Calculate the Ranking Points stats for the team eliminated position.
        ///// Calculate the Killing Points stats for the Player Killed.
        ///// Calculate the cumulative total points for the single Match.
        ///// Calculate the Daily Match Summary Ranking Order from the stats. 
        ///// </remarks>
        ///// <param name='matchId1'>f84d39a1-8218-4438-9bf5-7150f9e0f093</param>
        ///// <param name='matchId2'>869b9d5d-e99a-4455-b9b4-82087201d819</param>
        ///// <param name='matchId3'>7e65df95-be63-46a1-9aca-aca055fb6db5</param>
        ///// <param name='matchId4'>3fb26c4d-a73e-481b-88c9-af9aa1bfc003</param>
        //[HttpGet("DailySummaryRanking/{matchId1}/{matchId2}/{matchId3}/{matchId4}", Name = "GetDailySummaryRanking")]
        //public async Task<IEnumerable<DailyMatchRankingScore>> GetDailySummaryRanking(string matchId1, string matchId2, string matchId3, string matchId4)
        //{
        //  return  await _ranking.GetSummaryRanking(matchId1, matchId2, matchId3, matchId4);
        //}

        /// <summary>
        /// Returns Tournament Rankings         
        /// </summary>
        /// <remarks>
        /// Sample request: TournamentRankings        
        /// </remarks>        
        [HttpGet("TournamentRankings", Name = "GetTournamentRanking")]
        public async Task<IEnumerable<TournamentRanking>> GetTournamentRanking()
        {
            return await _ranking.GetTournamentRankings();
        }
      
        [HttpGet("TournamentRankingsByDay/{day}", Name = "GetTournamentRankingByDay")]
        public Task<IEnumerable<RankingResults>> GetTournamentRankingByDay(int day)
        {
            return _ranking.GetTournamentRankingByDay(day);
        }
    }
}
