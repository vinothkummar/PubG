using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model;
using Newtonsoft.Json.Linq;

namespace Fanview.API.Repository.Interface
{
    public interface ITakeDamageRepository
    {
        void InsertTakeDamageTelemetry(string jsonResult);
        void InsertEventDamageTelemetry(JObject[] jsonResult, string fileName);

        Task<IEnumerable<TakeDamage>> GetPlayerTakeDamage();
    }
}
