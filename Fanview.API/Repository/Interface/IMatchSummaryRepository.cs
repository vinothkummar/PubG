using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchSummaryRepository
    {
        void PollMatchSummary(string matchId);
        void PollMatchParticipantStats(string matchId);
        void CreateAndMapTestTeamPlayerFromMatchHistory(string matchId);
        Task PollMatchRoundRankingData(string matchId);     
        Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId);
        Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId1, string matchId2, string matchId3, string matchId4);
        void InsertLiveEventMatchStatusTelemetry(JObject[] jsonResult, string fileName);
        Task<IEnumerable<LiveMatchStatus>> GetLiveMatchStatus(int matchId);
    }
}
