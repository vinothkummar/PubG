﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class LiveDamageList
    {
        //public string MatchName { get; set; }
        //public int MatchId { get; set; }
        public IEnumerable<DamageList> DamageList {get; set;}

    }
}
