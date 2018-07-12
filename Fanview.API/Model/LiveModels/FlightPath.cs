using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class FlightPath
    {
        public string MatchName { get; set; }
        public int MatchId { get; set; }
        public Livelocation FlightPathStart { get; set; }
        public Livelocation FlightPathEnd { get; set; }
    }
}
