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
        public int playerID { get; set; }
        public string playerName { get; set; }
        public int teamid { get; set; }
        public string teamName { get; set; }
        public playerstats livePlayerStats { get; set; }
        public LiveTeamStats liveteamstat { get; set; }
    }
}
