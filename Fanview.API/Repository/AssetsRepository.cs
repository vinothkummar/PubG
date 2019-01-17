using System.Collections.Generic;
using System.IO;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using Fanview.API.Utility;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Fanview.API.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly ILogger<AssetsRepository> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly string _damageCausersResourcePath;
        private readonly string _damageCausersCacheKey;
        private readonly TimeSpan _damageCausersCacheExpiration;

        public AssetsRepository(ILogger<AssetsRepository> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _damageCausersResourcePath = "Fanview.API.Assets.DamageCauserName.json";
            _damageCausersCacheKey = "DamageCausers";
            _damageCausersCacheExpiration = TimeSpan.FromHours(2);
        }      

        public string GetDamageCauserName(string damageCauserKey)
        {
            var cached = _memoryCache.Get<string>(_damageCausersCacheKey);
            if (cached != null)
            {
                return cached;
            }
            try
            {
                var damageCausersJson = EmbeddedResourcesUtility.ReadEmbeddedResource(_damageCausersResourcePath);
                var damageCausersDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(damageCausersJson);
                var res = damageCausersDict[damageCauserKey];
                _memoryCache.Set(_damageCausersCacheKey, res, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(_damageCausersCacheExpiration)
                });
                return res;
            }
            catch (FileNotFoundException)
            {
                _logger.LogError($"Resource path: {_damageCausersResourcePath} not found");
            }
            catch (JsonReaderException)
            {
                _logger.LogError($"Failed to deserialize Damage Causers json into a dictionary.");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError($"Key {damageCauserKey} not found in the damage causers dictionary.");
            }
            return string.Empty;
        }
    }
}
