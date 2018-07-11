namespace Fanview.API.Model.LiveModels
{
    public class LiveTeamStats
    {
        public string MatchName { get; set; }
        public int MatchId { get; set; }
        public int teamId { get; set; }
        public string teamName { get; set; }
        public playerstats stats { get; set; }


    }
}
