using System.Collections.Generic;

namespace Fanview.API.Model
{
    public class Route
    {
        public int TeamID { get; set; }
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
        public IEnumerable<Location> TeamRoute { get; set; }
    }
}