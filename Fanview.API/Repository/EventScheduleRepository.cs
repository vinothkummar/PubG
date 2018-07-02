using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Fanview.API.Repository
{
    public class EventScheduleRepository : IEventScheduleRepository
    {
        private IGenericRepository<EventInfo> _eventInfoRepository;
        private ILogger<EventScheduleRepository> _logger;

        public EventScheduleRepository(IGenericRepository<EventInfo> eventInfoRepository,
                               ILogger<EventScheduleRepository> logger)
        {
            _eventInfoRepository = eventInfoRepository;
            _logger = logger;
        }
        public void CreateMultipleEventGameSchedule(List<EventInfo> eventInfos)
        {
            _eventInfoRepository.Insert(eventInfos, "EventScheduleInfo");
        }

        public async Task<EventInfo> GetDailySchedule(string daycount)
        {
            var dailySchedule = GetTournamentEventSchedule().SingleOrDefault(cn => cn.DayCount.ToLower() == daycount.ToLower());

            return await Task.FromResult(dailySchedule);
        }

        public async Task<Object> GetScheduledEvents()
        {
           var scheduleEvents = GetTournamentEventSchedule().Select(s => new
            {
                ScheduledDate = s.ScheduleDateTime.FirstOrDefault().ToString("MMM-dd"),
                GamePerspective = s.GamePerspective,
                DayCount = s.DayCount,
                Rounds = "4 ROUNDS"
            });

            return await Task.FromResult(scheduleEvents);

        }
        
        private IEnumerable<EventInfo> GetTournamentEventSchedule()
        {
            var tournamentEventScheduleInfo = new List<EventInfo>() {
                new EventInfo(){
                    ScheduleDateTime = new List<DateTime>()
                    {
                        new DateTime(2018,07,25,09,00,00),
                        new DateTime(2018,07,25,11,00,00),
                        new DateTime(2018,07,25,13,00,00),
                        new DateTime(2018,07,25,15,00,00)
                    },
                    DayCount = "Day-1",
                    GamePerspective = "TPP",
                    Name = "PubG 2018 Global Invitation",
                    MatchRoundStatus = MatchRoundStatus.NotStarted
                    },
                new EventInfo(){
                      ScheduleDateTime = new List<DateTime>()
                    {
                        new DateTime(2018,07,26,09,00,00),
                        new DateTime(2018,07,26,11,00,00),
                        new DateTime(2018,07,26,13,00,00),
                        new DateTime(2018,07,26,15,00,00)
                    },
                    DayCount = "Day-2",
                    GamePerspective = "TPP",
                    Name = "PubG 2018 Global Invitation",
                    MatchRoundStatus = MatchRoundStatus.NotStarted
                    },
                new EventInfo(){
                      ScheduleDateTime = new List<DateTime>()
                    {
                        new DateTime(2018,07,27,09,00,00),
                        new DateTime(2018,07,27,11,00,00),
                        new DateTime(2018,07,27,13,00,00),
                        new DateTime(2018,07,27,15,00,00)
                    },
                    DayCount = "Event-Matches",
                    GamePerspective = "",
                    Name = "PubG 2018 Global Invitation",
                    MatchRoundStatus = MatchRoundStatus.NotStarted
                    },
                new EventInfo(){
                      ScheduleDateTime = new List<DateTime>()
                    {
                        new DateTime(2018,07,28,09,00,00),
                        new DateTime(2018,07,28,11,00,00),
                        new DateTime(2018,07,28,13,00,00),
                        new DateTime(2018,07,28,15,00,00)
                    },
                    DayCount = "Day-3",
                    GamePerspective = "FPP",
                    Name = "PubG 2018 Global Invitation",
                    MatchRoundStatus = MatchRoundStatus.NotStarted
                    },
                new EventInfo(){
                    ScheduleDateTime = new List<DateTime>()
                    {
                        new DateTime(2018,07,29,09,00,00),
                        new DateTime(2018,07,29,11,00,00),
                        new DateTime(2018,07,29,13,00,00),
                        new DateTime(2018,07,29,15,00,00)
                    },
                    DayCount = "Day-4",
                    GamePerspective = "FPP",
                    Name = "PubG 2018 Global Invitation",
                    MatchRoundStatus = MatchRoundStatus.NotStarted
                    }
            };

            return tournamentEventScheduleInfo;
        }
    }
}
