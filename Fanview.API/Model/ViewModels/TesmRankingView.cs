using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class TeamRankingView
    {
        public string TeamId { get; set; }
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public int Kill { get; set; }
        public float Damage { get; set; }
        public int TotalPoints { get; set; }
        public int MatchId { get; set; }
    }
}
