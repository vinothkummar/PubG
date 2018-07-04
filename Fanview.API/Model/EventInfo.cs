using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class EventInfo
    {
        public string Name { get; set; }
        public string DayCount { get; set; }
        public string GamePerspective { get; set; }
        public List<MatchDailyRoundStatus> ScheduleTimeAndStatus { get; set; }
    }

}
