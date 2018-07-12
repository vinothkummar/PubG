using System;

namespace Fanview.API.Model.LiveModels
{
    public class DamageList
    {
        public string MatchName { get; set; }
        public int MatchID { get; set; }
        public double PlayerRank { get; set; }
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public int kills { get; set; }
        public double DamageDealt { get; set; }
        public int TeamId { get; set; }
        public TimeSpan SurvivlTime { get; set; }

    }
}