using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class TeamRoute
    {
        public string MatchName { get; set; }
        public int MatchId { get; set; }
        public Route Routs { get; set; }
        public Safezone[] Safezones { get; set; }
        public string EventTime { get; set; }
    }
}
