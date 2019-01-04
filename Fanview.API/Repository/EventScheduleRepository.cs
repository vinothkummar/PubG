using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Repository
{
    public class EventScheduleRepository : IEventScheduleRepository
    {
        private IHostingEnvironment _hostingEnvironment;
        private IGenericRepository<EventInfo> _eventInfoRepository;
        private ILogger<EventScheduleRepository> _logger;

        public EventScheduleRepository(IGenericRepository<EventInfo> eventInfoRepository,
                               ILogger<EventScheduleRepository> logger,
                               IHostingEnvironment hostingEnvironment
                               )
        {
            _hostingEnvironment = hostingEnvironment;
            _eventInfoRepository = eventInfoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Competition>> GetCompetitionSchedule()
        {
            var fileName = _hostingEnvironment.ContentRootPath + "\\Assets\\Schedule.json";

            var competion = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Competition>>(File.ReadAllText(fileName));

            return await Task.FromResult(competion);

           
        }
        public async Task<Competition> GetDailySchedule(string daycount)
        {
            var dailySchedule = GetCompetitionSchedule().Result.SingleOrDefault(cn => cn.DayCount.ToLower() == daycount.ToLower());

            return await Task.FromResult(dailySchedule);
        }
    }
}
