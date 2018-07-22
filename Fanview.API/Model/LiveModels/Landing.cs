using System.Collections.Generic;

namespace Fanview.API.Model.LiveModels
{
    public class Landing
    {
        public string TeamName { get; set; }
        public int TeamID { get; set; }
        public IEnumerable<LiveVeichleTeamPlayers> Players { get; set; }

    }
}
