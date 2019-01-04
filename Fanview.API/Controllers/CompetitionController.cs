using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<Competition> GetDailySchedule(string dayCount)
        {
            return await _eventScheduleRepository.GetDailySchedule(dayCount);
        }

        [HttpGet("Schedule", Name = "GetDailySchedules")]
        public async Task<IEnumerable<Competition>> GetDailySchedules()
        {
            return await _eventScheduleRepository.GetCompetitionSchedule();
        }
    }
}
