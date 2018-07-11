﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class KillZone
    {
        public string MatchName { get; set; }
        public int MatchId { get; set; }
        public Kill kills { get; set; }
        public string KillerName { get; set; }
        public Location Location { get; set; }
    }
}
