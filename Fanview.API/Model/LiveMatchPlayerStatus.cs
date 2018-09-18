using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveMatchPlayerStatus
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public bool IsALive { get; set; }

    }
}
