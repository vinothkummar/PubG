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
            var result = Task.FromResult(dailySchedule);
            return await result;
           
        }

        public async Task<Object> GetScheduledEvents()
        {
           var scheduleEvents = GetTournamentEventSchedule().Select(s => new
            {
                ScheduledDate = s.ScheduleTimeAndStatus.Select(t => t.ScheduleTime).First().ToString("MMM-dd"),
                GamePerspective = s.GamePerspective,
                DayCount = s.DayCount,
                Rounds = "4 ROUNDS"
            });

            return await Task.FromResult(scheduleEvents);

        }
        
        private IEnumerable<EventInfo> GetTournamentEventSchedule()
        {
            var early_Morning = new TimeSpan(9, 00, 00);
            var late_Morning = new TimeSpan(11, 00, 00);
            var late_Afternoon = new TimeSpan(15, 00, 00);
            var early_Afternoon = new TimeSpan(13, 00, 00);
            var finish_time = new TimeSpan(18, 00, 00);
           var tournamentEventScheduleInfo = new List<EventInfo>() {
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,09,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,11,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,13,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,15,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                    },
                    DayCount = "Day-1",
                    GamePerspective = "TPP",
                    Name = "PubG 2018 Global Invitation"
                    },
                new EventInfo(){
                     ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,09,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,11,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,13,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,15,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                    },
                    DayCount = "Day-2",
                    GamePerspective = "TPP",
                    Name = "PubG 2018 Global Invitation"
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,09,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,11,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,13,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,15,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                    },                     
                    DayCount = "Event-Matches",
                    GamePerspective = "",
                    Name = "PubG 2018 Global Invitation"                    
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,09,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,11,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,13,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,15,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                    },
                    DayCount = "Day-3",
                    GamePerspective = "FPP",
                    Name = "PubG 2018 Global Invitation"                   
                    },
                new EventInfo(){
                   ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,09,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,11,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,13,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,15,00,00), matchRoundStatus = MatchRoundStatus.Scheduled},
                    },
                    DayCount = "Day-4",
                    GamePerspective = "FPP",
                    Name = "PubG 2018 Global Invitation"                   
                    }
            };
            foreach (var item in tournamentEventScheduleInfo)
            {
                
                    var myrange = Enumerable.Range(25, 29);
                    var date = DateTime.Now.Date;
                    var day = date.Day;
                    var time =DateTime.Now - date;
                    
                    if (time <= early_Morning)
                    {
                        item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Next;
                        item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Scheduled;
                        item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Scheduled;
                        item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Scheduled;
                        break;
                    }
                   else if (time > early_Morning && time <= late_Morning)
                    {

                        item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Active;
                        item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Next;
                        item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Scheduled;
                        item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Scheduled;
                        break;


                    }
                    else if (time > late_Morning && time <= early_Afternoon)
                    {

                        item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Active;
                        item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Next;
                        item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Scheduled;
                        break;

                    }
                   else if (time > early_Afternoon && time <= late_Afternoon)
                    {

                        item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Active;
                        item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Next;
                        break;


                    }
                else if (time >late_Afternoon && time <= finish_time)
                {

                    item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Completed;
                    item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Completed;
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Completed;
                    item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Active;
                    break;


                }
                else
                    {
                        item.ScheduleTimeAndStatus[0].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[1].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[2].matchRoundStatus = MatchRoundStatus.Completed;
                        item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Completed;
                        break;

                    }
                }
            
            return tournamentEventScheduleInfo;
        }
    }
}
