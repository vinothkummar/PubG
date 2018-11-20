using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class PlayerAll
    {       
        public string Id { get; set; }
        public int TeamId { get; set;}
        public string longTeamId { get; set; }
        public int PlayerId { get; set; }      
        public string PlayerName { get; set; }       
        public string FullName { get; set; }       
        public string Country { get; set; }        
       
    }
}
