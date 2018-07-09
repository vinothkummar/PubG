using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;


namespace Fanview.API.BusinessLayer
{
    public class ReadAssets : IReadAssets
    {
        private IHostingEnvironment _hostingEnvironment;
        public ReadAssets(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string GetDamageCauserName(string damageCauserKey)
        {
            var fileName = _hostingEnvironment.ContentRootPath + "\\Assets\\DamageCauserName.json";
            //var fileName = Path.GetFullPath(@"../" + "Fanview.API\\Assets\\DamageCauserName.json");

            JObject damageCauserNameList = JObject.Parse(File.ReadAllText(fileName));

            var damagedCaused = damageCauserNameList.SelectToken(damageCauserKey);

            return damagedCaused.ToString();

        }

    }
}
