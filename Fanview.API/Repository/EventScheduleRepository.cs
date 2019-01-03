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

        //public async Task<Object> GetScheduledEvents()
        //{
        //   var scheduleEvents = GetTournamentEventSchedule().Select(s => new
        //    {
        //        ScheduledDate = s.ScheduleTimeAndStatus.Select(t => t.ScheduleTime).First().ToString("yyyy-MM-dd"),
        //        GamePerspective = s.GamePerspective,
        //        DayCount = s.DayCount,
        //        Rounds = s.DayCount == "3"? "Event Matches" : "Day " +s.DayCount + " - 4 Rounds" 
        //    });
        //    var ScheduleEventsNameIncluded = new { EventName = "PUBG Global Invitational Berlin 2018", EventSchedule = scheduleEvents };
        //    return await Task.FromResult(ScheduleEventsNameIncluded);

        //}
        
        private IEnumerable<EventInfo> GetTournamentEventSchedule()
        {
            var tournamentEventScheduleInfo = new List<EventInfo>() {
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Completed),MatchId=1, PubgMatchid="b2224f16-099e-4f1b-9869-70c17afb77ea"},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Completed),MatchId=2, PubgMatchid="be4a650e-9459-493e-915b-6f2f7057ea28"},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Completed),MatchId=3, PubgMatchid="789a5d81-8e19-4035-aed3-e3b7562e3dfd"},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,25,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Completed),MatchId=4, PubgMatchid="5f6babee-e2c4-46ea-9e7e-5ee21db1cbc2"},                    
                                
                    },
                    DayCount = "1",
                    GamePerspective = "TPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                     ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {


                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Completed),MatchId=5, PubgMatchid="038e363c-3811-4a1d-bafc-82b631d0afbb"},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.InProgress),MatchId=6, PubgMatchid="675619e6-4a11-6b92-cf2e-4c82428b78ef"},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=7, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,26,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=8, PubgMatchid=""},

                    },
                    DayCount = "2",
                    GamePerspective = "TPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=9, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,18,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=10, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,19,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=11, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,27,20,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=12, PubgMatchid=""},
                    },                     
                    DayCount = "3",
                    GamePerspective = "FPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                    ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,14,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=13, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,15,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=14, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,16,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=15, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,28,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=16, PubgMatchid=""},
                    },
                    DayCount = "4",
                    GamePerspective = "FPP",
                    Name = "PUBG Global Invitational Berlin 2018"
                    },
                new EventInfo(){
                   ScheduleTimeAndStatus = new List<MatchDailyRoundStatus>()
                    {
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,14,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=17, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,15,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=18, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,16,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=19, PubgMatchid=""},
                        new MatchDailyRoundStatus(){ScheduleTime = new DateTime(2018,07,29,17,00,00), MatchRoundStatus = nameof(MatchRoundStatus.Scheduled),MatchId=20, PubgMatchid=""},
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
