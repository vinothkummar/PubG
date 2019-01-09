using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class RankingResults
    {
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public int KillPoints { get; set; }
        public int RankPoints { get; set; }
        public int TotalPoints { get; set; }
        public int TeamId { get; set; }
        public int GameTeamId { get; set; }
    }
}
