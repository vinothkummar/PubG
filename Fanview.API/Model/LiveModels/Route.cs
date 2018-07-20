namespace Fanview.API.Model.LiveModels
{
    public class Route
    {
        public string TeamID { get; set; }
        public int TeamRank { get; set; }
        public string TeamName { get; set; }
        public Location TeamRoute { get; set; }
        public Safezone Safezone { get; set; }
        public string PlayerName { get; set; }
    }
}