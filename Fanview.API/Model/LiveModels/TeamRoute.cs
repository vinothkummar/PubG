using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.LiveModels
{
    public class TeamRoute
    {
        public string TeamName { get; set; }
        public int RoundId { get; set; }
        public Route routs { get; set; }
        public Safezone[] Safezones { get; set; }
    }
}
