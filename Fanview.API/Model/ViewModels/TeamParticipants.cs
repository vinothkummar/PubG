using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class TeamParticipants
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }      
        public List<Participants> TeamPlayer { get; set; }
    }
}
