using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class KillMessage
    {
        public Killer Killer { get; set; }
        public Victim Victim { get; set; }       
        public string DamageTypeCategory { get; set; }
        public string DamageReason { get; set; }
        public string DamageCauserName { get; set; }  
        public string EventTimeStamp { get; set; } 
    }
}
