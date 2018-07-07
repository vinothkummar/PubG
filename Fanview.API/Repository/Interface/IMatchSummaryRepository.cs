using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchSummaryRepository
    {
        void PollMatchSummary(string matchId);
        void PollMatchParticipantStats(string matchId);
        void CreateAndMapTestTeamPlayerFromMatchHistory(string matchId);
        Task<IEnumerable<MatchPlayerStats>> GetPlayerMatchStats(string matchId);
    }
}
