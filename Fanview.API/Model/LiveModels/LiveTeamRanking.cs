namespace Fanview.API.Model.LiveModels
{
    public class LiveTeamRanking
    {
        public int TeamId { get; set; }
        public int TeamRank { get; set; }
        public string TeamName { get; set; }
        public int KillPoints { get; set; }
        public int RankPoints { get; set; }
        public int TotalPoints { get; set; }
    }
}
