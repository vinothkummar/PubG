using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Model.LiveModels
{
    public class KillZone
    {
        public string MatchName { get; set; }
        public string MatchId { get; set; }
        public string KillerName { get; set; }
        public int KillerId { get; set; }
        public int TeamId { get; set; }
        public LiveLocation location { get; set; }
    }
}
