using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class DeskSeatings
    {       
        public int DeskNumber { get; set; }       
        public int TeamIdShort { get; set; }       
        public int Seat1PlayerId { get; set; }       
        public int Seat2PlayerId { get; set; }        
        public int Seat3PlayerId { get; set; }        
        public int Seat4PlayerId { get; set; }
    }
}
