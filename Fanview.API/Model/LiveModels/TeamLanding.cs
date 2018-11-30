using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class TeamLanding
    {
        public string MatchdId { get; set; }
        public string MapName { get; set; }
        public IEnumerable<Landing> Landing { get; set; }
    }

}
