using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class LiveTeamPlayers
    {
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public LiveLocation location { get; set; }
        public Boolean PlayerStatus { get; set; }
        public int PlayerTeamId { get; set; }
        public string TimeKilled { get; set; }
    }

    public class LiveVeichleTeamPlayers
    {
        public string PlayerName { get; set; }
        public int? PlayerId { get; set; }
        public LiveLocation location { get; set; }
        
    }
}
