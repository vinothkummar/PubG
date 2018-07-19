using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Model.LiveModels
{
    public class KillZone
    {   
        public string MatchId { get; set; }
        public string PlayerName { get; set; }
        public int TeamId { get; set; }
        public float Health { get; set; }        
        public Location Location { get; set; }
    }
}
