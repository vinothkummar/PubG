using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class TeamLineUpPlayers
    {         
        public string PlayerName { get; set; }      
        public string PubgAccountId { get; set; }
        public int Kills { get; set; }
        public int TimeSurvived { get; set; }
    }
}
