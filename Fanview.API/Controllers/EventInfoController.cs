using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventInfoController : ControllerBase
    {
        private IEventScheduleRepository _eventScheduleRepository;

        public EventInfoController(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }
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

        [HttpGet("Schedule/{dayCount}", Name = "GetDailySchedule")]
        public async Task<EventInfo> GetDailySchedule(string dayCount="Day-1")
        {
            return await _eventScheduleRepository.GetDailySchedule(dayCount);
        }

        [HttpGet("Schedule/Event", Name = "GetScheduleEvents")]
        public async Task<Object> GetScheduleEvents()
        {
            return await _eventScheduleRepository.GetScheduledEvents();
        }
    }
}
