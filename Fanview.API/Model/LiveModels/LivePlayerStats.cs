using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class LivePlayerStats
    {
        public string MatchName { get; set; }
        public string MatchdID { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public int Teamid { get; set; }
        public string TeamName { get; set; }
        public PlayerStats PlayerStats { get; set; }
        public LiveTeamStats TeamStats { get; set; }
    }
}
