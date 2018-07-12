using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Model.LiveModels
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TeamPlayer> TeamPlayers { get; set; }
    }
}
