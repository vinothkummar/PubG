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
       


        public IEnumerable<EventInfo> EventSchedule(IEnumerable<EventInfo> scheduledinfo)
        {
            var early_Morning = new TimeSpan(9, 00, 00);
            var late_Morning = new TimeSpan(11, 00, 00);
            var early_Afternoon = new TimeSpan(13, 00, 00);
            var late_Afternoon = new TimeSpan(15, 00, 00);
            var finish_time = new TimeSpan(18, 00, 00);


            foreach (var item in scheduledinfo)
            {

                var myrange = Enumerable.Range(25, 30);
                var date = DateTime.Now.Date;
                var day = date.Day;
                var time = DateTime.Now - date;

                if (time < early_Morning)
                {

                    item.ScheduleTimeAndStatus[0].matchRoundStatus =nameof( MatchRoundStatus.Next);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus =nameof( MatchRoundStatus.Scheduled);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus =nameof( MatchRoundStatus.Scheduled);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus= nameof(MatchRoundStatus.Scheduled);
                    break;
                }
                else if (time > early_Morning && time <= late_Morning)
                {

                    item.ScheduleTimeAndStatus[0].matchRoundStatus = nameof(MatchRoundStatus.Active);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus= nameof(MatchRoundStatus.Next);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = nameof(MatchRoundStatus.Scheduled);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus = nameof(MatchRoundStatus.Scheduled);
                    break;


                }
                else if (time > late_Morning && time <= early_Afternoon)
                {

                    
                    item.ScheduleTimeAndStatus[0].matchRoundStatus= nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus = nameof(MatchRoundStatus.Active);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = nameof(MatchRoundStatus.Next);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus = nameof(MatchRoundStatus.Scheduled);
                    break;

                }
                else if (time > early_Afternoon && time <= late_Afternoon)
                {

                 
                    item.ScheduleTimeAndStatus[0].matchRoundStatus= nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus= nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = nameof(MatchRoundStatus.Active);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus= nameof(MatchRoundStatus.Next);
                    break;


                }
                else if (time > late_Afternoon && time <= finish_time)
                {

                   
                    item.ScheduleTimeAndStatus[0].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus= nameof(MatchRoundStatus.Active);
                    break;


                }
                else
                {
                    
                    item.ScheduleTimeAndStatus[0].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[1].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[2].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    item.ScheduleTimeAndStatus[3].matchRoundStatus = nameof(MatchRoundStatus.Completed);
                    break;

                }
            }
            
            return scheduledinfo;

        }

    }
}