using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class TeamLanding
    {
        public string MatchName { get; set; }
        public int MatchdId { get; set; }
        public Landing Landing { get; set; }
    }
}
