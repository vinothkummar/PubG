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

        public async Task<IEnumerable<EventInfo>> GetDailySchedule()
        {
            var dailySchedule = GetTournamentEventSchedule();

            return await Task.FromResult(dailySchedule);
        }

        public async Task<Object> GetScheduledEvents()
        {
           var scheduleEvents = GetTournamentEventSchedule().Select(s => new
            {
                ScheduledDate = s.ScheduleTimeAndStatus.Select(t => t.ScheduleTime).First().ToString("yyyy-MM-dd"),
                GamePerspective = s.GamePerspective,
                DayCount = s.DayCount,
                Rounds = s.DayCount == "3"? "Event Matches" : "Day " +s.DayCount + " - 4 Rounds" 
            });
            var ScheduleEventsNameIncluded = new { EventName = "PUBG Global Invitational Berlin 2018", EventSchedule = scheduleEvents };
            return await Task.FromResult(ScheduleEventsNameIncluded);

        }
        
        private IEnumerable<EventInfo> GetTournamentEventSchedule()
        {
            var tournamentEventScheduleInfo = new List<EventInfo>() {
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=1},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=2},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=3},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=4},                    
                                
                    },
                    DayCount = "1",
                    GamePerspective = "TPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                     ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {


                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=5},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=6},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=7},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=8},                       
                        
                    },
                    DayCount = "2",
                    GamePerspective = "TPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=9},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=10},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=11},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=12},
                    },                     
                    DayCount = "3",
                    GamePerspective = "FPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,14,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=13},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,15,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=14},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,16,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=15},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=16},
                    },
                    DayCount = "4",
                    GamePerspective = "FPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                   ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,14,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=17},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,15,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=18},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,16,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=19},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=20},
                    },
                    DayCount = "5",
                    GamePerspective = "FPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    }
            };

            return tournamentEventScheduleInfo;
        }
    }
}
