using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class ScheduleTimeAndStatus
    {
        public DateTime ScheduleTime { get; set; }
        public string MatchRoundStatus { get; set; }
        public string stageId { get; set; }
        public string matchId { get; set; }
    }
}
