using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class TeamLineUp
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public int TeamRank { get; set; }
        public List<TeamLineUpPlayers> TeamPlayer { get; set; }
    }
}
