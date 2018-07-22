using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class LiveKillCount
    {
        public string KillerName { get; set; }
        public int  KillerTeamId { get; set; }
        public int KillCount { get; set; }
    }
}
