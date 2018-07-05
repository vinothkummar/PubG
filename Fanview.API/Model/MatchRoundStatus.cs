﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public enum MatchRoundStatus
    {   
        None,
        NotStarted,        
        Active,        
        Completed,
        Next,
        Scheduled
    }
}