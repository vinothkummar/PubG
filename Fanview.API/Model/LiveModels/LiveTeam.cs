using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Model.LiveModels
{
    public class LiveTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LiveTeamPlayers TeamPlayers { get; set; }
    }
}
