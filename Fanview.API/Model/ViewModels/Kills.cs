using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class Kills
    {
        public string playerName { get; set; }
        public float playerId { get; set; }
        public int kills { get; set; }
        public int timeSurvived { get; set; }
        public int teamId { get; set; }
    }
}
