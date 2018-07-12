namespace Fanview.API.Model.LiveModels
{
    public class LiveTeamStats
    {
        public string MatchName { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public PlayerStats stats { get; set; }

    }
}
