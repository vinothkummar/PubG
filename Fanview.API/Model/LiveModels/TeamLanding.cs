using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class TeamLanding
    {
        public string MatchdId { get; set; }
        public List<Landing> Landing { get; set; }
    }

}
