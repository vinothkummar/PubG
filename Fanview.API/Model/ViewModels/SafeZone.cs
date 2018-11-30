using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class SafeZone
    {
        public int circle;

        public SafetyZonePosition SafetyZonePosition { get; set; }
       
        public float SafetyZoneRadius { get; set; }
    }
}
