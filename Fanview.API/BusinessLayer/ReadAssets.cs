using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Microsoft.Extensions.Logging;

namespace Fanview.API.BusinessLayer
{
    public class ReadAssets : IReadAssets
    {
        private IHostingEnvironment _hostingEnvironment;
        private ILogger<ReadAssets> _logger;

        public ReadAssets(IHostingEnvironment hostingEnvironment, ILogger<ReadAssets> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            
        }
        public string GetDamageCauserName(string damageCauserKey)
        {
            var fileName = _hostingEnvironment.ContentRootPath + "\\Assets\\DamageCauserName.json";
            //var fileName = Path.GetFullPath(@"../" + "Fanview.API\\Assets\\DamageCauserName.json");

            JObject damageCauserNameList = JObject.Parse(File.ReadAllText(fileName));

            try
            {
                var damagedCaused = damageCauserNameList.SelectToken(damageCauserKey);

                return damagedCaused.ToString();
            }
            catch (Exception exception)
            {
                _logger.LogInformation("Damage Causer key " + exception + Environment.NewLine);
                return null;
            }
            

           

        }

    }
}
