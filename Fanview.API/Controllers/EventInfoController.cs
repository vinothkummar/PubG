using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        /// <summary>
        ///   Returns Event Stadium Information      
        /// </summary>
        /// <remarks>
        /// Sample request: api/EventInfo/Location          
        /// No Input Parameters Required
        /// </remarks>       
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

        /// <summary>
        /// Returns Match Daily Schedule and the Round Information     
        /// </summary>
        /// <remarks>
        /// Sample request: api/EventInfo/{dayCount}          
        /// Input Parameters: day-1; day-2; day-3; day-4
        /// </remarks>
        /// <param name='dayCount'>day-1</param>
        [HttpGet("Schedule/{dayCount}", Name = "GetDailySchedule")]
        public async Task<EventInfo> GetDailySchedule(string dayCount)
        {
            return await _eventScheduleRepository.GetDailySchedule(dayCount);
        }

        /// <summary>
        /// Returns Tournament Schedule and the Game Perspective   
        /// </summary>
        /// <remarks>
        /// Sample request: api/EventInfo/Schedule/Event          
        /// No Input Parameter required
        /// </remarks>
        [HttpGet("Schedule/Event", Name = "GetScheduleEvents")]
        public async Task<Object> GetScheduleEvents()
        {
            return await _eventScheduleRepository.GetScheduledEvents();
        }
    }
}
