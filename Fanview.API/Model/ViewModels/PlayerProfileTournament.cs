using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class PlayerProfileTournament
    {
        public string  MatchId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public int teamId { get; set; }
        public Stats stats { get; set; }

    }
}
