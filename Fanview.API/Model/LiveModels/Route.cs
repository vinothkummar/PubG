namespace Fanview.API.Model.LiveModels
{
    public class Route
    {
        public string TeamID { get; set; }
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public LiveLocation TeamRoute { get; set; }
        public Safezone Safezone { get; set; }
    }
}