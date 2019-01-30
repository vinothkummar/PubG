using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class Competition
    {
        public string CompetitionName { get; set; }
        public string WeekCount { get; set; }
        public string DayCount { get; set; }
        public IEnumerable<ScheduleTimeAndStatus> ScheduleTimeAndStatus { get; set; }
    
    }
}
