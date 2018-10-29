using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Services.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Fanview.API.Services
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _cache;

        public CacheService(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }
        public async Task<T> RetrieveFromCache<T>(string key)
        {
            var json = await _cache.GetStringAsync(key);

            if (json != null)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return default(T);
            }
           
        }

        public async Task SaveToCache<T>(string key, T item, int absoluteExpirationRelativeToNow, int slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(item);

            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration)

            });
        }
    }
}
