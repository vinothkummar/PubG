﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class PlayerKilledGraphics
    {
        public string TimeKilled { get; set; }
        public string KillerName { get; set; }
        public string FreeText1 { get; set; }
        public string VictimName { get; set; }
        public string FreeText2 { get; set; }
        public string DamagedCausedBy { get; set; }
        public string PlayerLeft { get; set; }
    }
}