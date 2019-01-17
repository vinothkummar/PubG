using System.Collections.Generic;
using System.IO;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using Fanview.API.Utility;
using Newtonsoft.Json;

namespace Fanview.API.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private ILogger<AssetsRepository> _logger;

        public AssetsRepository(ILogger<AssetsRepository> logger)
        {
            _logger = logger;
        }      

        public string GetDamageCauserName(string damageCauserKey)
        {
            const string damageCauserResourcePath = "Fanview.API.Assets.DamageCauserName.json";
            try
            {
                var damageCausersJson = EmbeddedResourcesUtility.ReadEmbeddedResource(damageCauserResourcePath);
                var damageCausersDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(damageCausersJson);
                return damageCausersDict[damageCauserKey].ToString();
            }
            catch (FileNotFoundException)
            {
                _logger.LogError($"Resource path: {damageCauserResourcePath} not found");
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
