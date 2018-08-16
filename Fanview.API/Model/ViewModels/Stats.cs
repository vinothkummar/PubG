using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class Stats
    {
        public int Knocks { get; set; }
        public int Assists { get; set; }
        public int Boosts { get; set; }
        public float damage{ get; set; }
        public int headShot { get; set; }
        public int Heals { get; set; }
        public int Kills { get; set; }
        public int TimeSurvived { get; set; }
        public int Revives { get; set; }
        public float RideDistance { get; set; }
        public float SwimDistance { get; set; }
        public float WalkDistance { get; set; }
    }
}
