using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class LiveKillCount
    {
        public int KillerTeamId { get; set; }
        public string KillerName { get; set; }
        public int KillCount { get; set; }
    }
}
