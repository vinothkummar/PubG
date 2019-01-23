using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fanview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private IEventScheduleRepository _eventScheduleRepository;

        public CompetitionController(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        /// <summary>
        /// Returns Match Daily Schedule and the Round Information     
        /// </summary>
        /// <remarks>
        /// Sample request: api/Competition/{dayCount}          
        /// Input Parameters: 1; 2; 3; 4
        /// </remarks>
        /// <param name='dayCount'>1</param>
        [HttpGet("Schedule/{dayCount}", Name = "GetDailySchedule")]
        public Competition GetDailySchedule(string dayCount)
        {
            return _eventScheduleRepository.GetDailySchedule(dayCount);
        }

        [HttpGet("Schedule", Name = "GetDailySchedules")]
        public IEnumerable<Competition> GetDailySchedules()
        {
            return _eventScheduleRepository.GetCompetitionSchedule();
        }
    }
}
