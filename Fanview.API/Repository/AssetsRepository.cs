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
            try
            {
                var damageCausersDict = _memoryCache.Get<Dictionary<string, string>>(_damageCausersCacheKey);
                if (damageCausersDict == null)
                {
                    var damageCausersJson = EmbeddedResourcesUtility.ReadEmbeddedResource(_damageCausersResourcePath);
                    damageCausersDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(damageCausersJson);
                    _memoryCache.Set(_damageCausersCacheKey, damageCausersDict, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.Add(_damageCausersCacheExpiration)
                    });
                }
                return damageCausersDict[damageCauserKey];
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
