using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanview.API.Repository
{
    public class EventScheduleRepository : IEventScheduleRepository
    {
        private ILogger<EventScheduleRepository> _logger;
        private readonly IMemoryCache _memoryCache;

        private readonly string _eventScheduleResourcePath;
        private readonly string _eventScheduleCacheKey;
        private readonly TimeSpan _eventScheduleCacheExpiration;

        public EventScheduleRepository(ILogger<EventScheduleRepository> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _eventScheduleResourcePath = "Fanview.API.Assets.Schedule.json";
            _eventScheduleCacheKey = "EventSchedule";
            _eventScheduleCacheExpiration = TimeSpan.FromHours(2);
        }

        public IEnumerable<Competition> GetCompetitionSchedule()
        {
            var schedule = _memoryCache.Get<IEnumerable<Competition>>(_eventScheduleCacheKey);
            if (schedule != null)
            {
                return schedule;
            }
            try
            {
                var scheduleJson = EmbeddedResourcesUtility.ReadEmbeddedResource(_eventScheduleResourcePath);
                var res = JsonConvert.DeserializeObject<IEnumerable<Competition>>(scheduleJson);
                _memoryCache.Set(_eventScheduleCacheKey, res, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(_eventScheduleCacheExpiration)
                });
                return res;
            }
            catch (FileNotFoundException)
            {
                _logger.LogError($"Resource path: {_eventScheduleResourcePath} not found");
            }
            catch (JsonReaderException)
            {
                _logger.LogError($"Failed to deserialize Event Schedule json into a dictionary.");
            }
            return null;
        }

        public Competition GetDailySchedule(string dayCount)
        {
            return GetCompetitionSchedule()
                .FirstOrDefault(cs => string.Equals(cs.DayCount, dayCount, 
                StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
