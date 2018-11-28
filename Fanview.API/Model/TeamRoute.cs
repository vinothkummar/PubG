using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class TeamRoute
    {  
        public int MatchId { get; set; }
        public string MapName { get; set; }
        public List<Route> Route { get; set; }
    }
}
