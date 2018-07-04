using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchSummaryRepository
    {
        void PollMatchSummary(string matchId);
        void PollMatchParticipantStats(string matchId);
    }
}
