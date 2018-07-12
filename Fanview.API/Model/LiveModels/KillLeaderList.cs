using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class KillLeaderList
    {
        public string MatchName { get; set; }
        public int MatchID { get; set; }
        public IEnumerable<DamageList> DamageLists { get; set; }
    }

    
}
