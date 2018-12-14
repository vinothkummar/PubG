using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Fanview.API.Assets;
using Fanview.API.Services.Interface;

namespace Fanview.API.Repository
{
    public class AssetsRepository : IAssetsRepository
    {
        private IHostingEnvironment _hostingEnvironment;
        private ILogger<AssetsRepository> _logger;
        private ICacheService _cacheService;


        public AssetsRepository(IHostingEnvironment hostingEnvironment, ILogger<AssetsRepository> logger, ICacheService cacheService)
        {
            _hostingEnvironment = hostingEnvironment;
            _cacheService = cacheService;
            _logger = logger;


        }      

        public string GetDamageCauserName(string damageCauserKey)
        {
            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            var contents = provider.GetDirectoryContents(string.Empty).Where(cn => cn.Name == "Assets").FirstOrDefault().PhysicalPath;

            var fileName = contents + "\\DamageCauserName.json";

            //var fileName = Path.GetFullPath(@"../" + "Fanview.API\\Assets\\DamageCauserName.json");

            JObject damageCauserNameList = JObject.Parse(File.ReadAllText(fileName));

            //var damageCauserNameList = new DamageCauserName(_cacheService).GetDamageCauserName().Result;

            try
            {
                var damagedCaused = damageCauserNameList.SelectToken(damageCauserKey);

                //var damagedCaused = damageCauserNameList.FirstOrDefault(cn => cn.Key == damageCauserKey).Value;

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
