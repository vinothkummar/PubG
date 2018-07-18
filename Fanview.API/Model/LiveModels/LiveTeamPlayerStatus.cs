using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class LiveTeamPlayerStatus
    {       
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<LiveTeamPlayers> TeamPlayers { get; set; }
    }
}
