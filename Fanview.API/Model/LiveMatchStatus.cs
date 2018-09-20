using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveMatchStatus
    {
        public int Id { get; set; }       
        public IEnumerable<LiveMatchPlayerStatus> TeamPlayers { get; set; }
        public int AliveCount { get; set; }
        public int DeadCount { get; set; }
        public bool IsEliminated { get; set; }
        public string EliminatedAt { get; set; }

    }
}
