using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchDailyRoundStatus
    {
        public DateTime ScheduleTime { get; set; }    
        public string MatchRoundStatus{ get; set; }
        public string PubgMatchid { get; set; }
        public int MatchId { get; set; }


    }
}
