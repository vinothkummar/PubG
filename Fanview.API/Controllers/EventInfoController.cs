using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventInfoController : ControllerBase
    {
        // GET: api/EventInfo
        [HttpGet("Location")]
        public EventLocation Get()
        {
            var location = new EventLocation();
            location.EventName = "PUBG_Global_Invitational 2018";
            location.EventPlace = "Mercedes-Benz Arena";
            location.Address1 = "Mercedes-Platz 1";
            location.PostCode = " 10243";
            location.State = "Berlin";
            location.Country = "Germany";

            return location;

        }
    }
}
