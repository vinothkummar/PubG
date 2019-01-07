using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace Fanview.API.Services
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _cache;
        private string[] _cacheKeys;

        public CacheService(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
             _cacheKeys = new string[]{ "TeamCountCache", "TeamPlayerCache", "TeamLiveStatusCountCach", "LiveEventKilledCache",
                                           "LiveKilledCache", "TeamLiveStatusCache", "TournamentMatchIdCache", "MatchRankPointsCache",
                                           "TournamentMatchCache", "TournamentMatchCreatedAtCache", "LiveKilledCache", "DamageCauserCache" //,"LiveTeamRanking"
                                         };
        }

        public void RefreshFromCache()
        {
            foreach (var item in _cacheKeys)
            {
                _cache.Refresh(item);
            }
            
        }

        public void RemoveFromCache()
        {
           
            foreach (var item in _cacheKeys)
            {
                _cache.Remove(item);
            }
             
        }

        public T RetrieveFromCache<T>(string key)
        { 

            var json = _cache.GetString(key);

            if (json == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> RetrieveObjFromCache<T>(string key) where T : class
        {
            var byteArray = await _cache.GetAsync(key);

            if (byteArray == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

        public async Task SaveObjToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration)
        {

             await _cache.SetAsync(key, ToByteArray(item), 
                 new DistributedCacheEntryOptions{
                 AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow),
                 SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
             });  
        }

        public async Task SaveToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(item);

            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(absoluteExpirationRelativeToNow),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)
            });
        }

        private byte[] ToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
    }
}
