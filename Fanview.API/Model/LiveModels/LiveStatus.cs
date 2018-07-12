using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;

namespace Fanview.API.Model.LiveModels
{
    public class LiveStatus
    {
        public string MatchName { get; set; }
        public int MatchID { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}
