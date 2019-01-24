using System.Collections.Generic;
using Fanview.API.Model;


namespace Fanview.API.Repository.Interface
{
    public interface IEventScheduleRepository
    {        
        Competition GetDailySchedule(string daycount);
        IEnumerable<Competition> GetCompetitionSchedule();
    }
}
