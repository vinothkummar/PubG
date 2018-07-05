using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;

namespace Fanview.API.BusinessLayer
{
    public class EventSchedule_logic: IEvent_logic
    {
        public TimeSpan early_Morning { get; set; } 
        public TimeSpan late_Morning { get; set; } 
        public TimeSpan late_Afternoon { get; set; } 
        public TimeSpan finish_time { get; set; }
        public TimeSpan early_Afternoon {get;set;}
        public EventSchedule_logic()
        {
            this.early_Morning= new TimeSpan(9, 00, 00);
            this.late_Morning= new TimeSpan(11, 00, 00);
            this.early_Afternoon = new TimeSpan(13, 00, 00);
            this.late_Afternoon = new TimeSpan(15, 00, 00);

        }
        public IEnumerable<EventInfo> EventSchedule(IEnumerable<EventInfo> myinfo)
        {
            foreach (var item in myinfo)
            {

                var myrange = Enumerable.Range(25, 30);
                var date = DateTime.Now.Date;
                var day = date.Day;
                var time = DateTime.Now - date;

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
                else if (time > late_Afternoon && time <= finish_time)
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
                    item.ScheduleTimeAndStatus[3].matchRoundStatus = MatchRoundStatus.Active;
                    break;

                }
            }
            return myinfo;

        }
    }
}