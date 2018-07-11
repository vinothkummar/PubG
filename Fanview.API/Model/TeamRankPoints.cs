using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class TeamRankPoints
    {
        public int Positions { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string TeamId { get; set; }
        public int OpenApiVictimTeamId { get; set; }
        public string  MatchId { get; set; }
        public string PlayerAccountId { get; set; }


    }
}
