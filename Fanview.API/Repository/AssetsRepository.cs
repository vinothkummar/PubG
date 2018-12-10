using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private IHostingEnvironment _hostingEnvironment;
        private ILogger<AssetsRepository> _logger;

        public AssetsRepository(IHostingEnvironment hostingEnvironment, ILogger<AssetsRepository> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;

        }      

        public string GetDamageCauserName(string damageCauserKey)
        {
            
            var fileName = _hostingEnvironment.ContentRootPath + "\\Project Workspace\\PubG.Solution\\UdpProcessor\\Assets\\DamageCauserName.json";
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
