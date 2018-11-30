using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class FlightPath
    {        
        public int MatchId { get; set; }
        public string MapName { get; set; }
        public Location FlightPathStart { get; set; }
        public Location FlightPathEnd { get; set; }
    }
}
