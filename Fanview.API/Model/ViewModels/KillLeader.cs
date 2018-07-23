using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Model.ViewModels
{
    public class KillLeader
    {
        //public int matchName { get; set; }
        public float matchId { get; set; }
        public IEnumerable<Kills> killList { get; set; }

    }
}