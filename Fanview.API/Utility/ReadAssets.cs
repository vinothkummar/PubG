using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Fanview.API.Utility
{
    public static class ReadAssets
    {   
        public static string GetDamageCauserName(string damageCauserKey)
        {
            var fileName = Path.GetFullPath(@"../" + "Fanview.API\\Assets\\DamageCauserName.json");

            JObject damageCauserNameList = JObject.Parse(File.ReadAllText(fileName));

            var damagedCaused = damageCauserNameList.SelectToken(damageCauserKey);

            return damagedCaused.ToString();

        }

    }
}
