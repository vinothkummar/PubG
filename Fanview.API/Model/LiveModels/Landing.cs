using System.Collections.Generic;

namespace Fanview.API.Model.LiveModels
{
    public class Landing
    {
        public string TeamName { get; set; }
        public string TeamID { get; set; }
        public IEnumerable<LiveTeamPlayers> Players { get; set; }

    }
}
