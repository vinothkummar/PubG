using Fanview.API.Model;
using Fanview.API.Repository.Interface;
using Fanview.API.Utility;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class TakeDamageRepository : ITakeDamageRepository
    {
        private DateTime killEventlastTimeStamp = DateTime.MinValue;
        private IGenericRepository<TakeDamage> _genericRepository;
        private ILogger<PlayerKillRepository> _logger;

        public TakeDamageRepository(IGenericRepository<TakeDamage> genericRepository, ILogger<PlayerKillRepository> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public Task<IEnumerable<TakeDamage>> GetPlayerTakeDamage()
        {
            var result = _genericRepository.GetAll("TakeDamage");

            return result;
        }

        public async void InsertTakeDamageTelemetry(string jsonResult)
        {
            var jsonToJObject = JArray.Parse(jsonResult);

            var lastestKillEventTimeStamp = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerTakeDamage").Select(s => new { EventTimeStamp = s.Value<string>("_D") }).Last();

            IEnumerable<TakeDamage> logPlayerTakeDamage = GetLogPlayerTakeDamage(jsonToJObject);

            var killEventTimeStamp = logPlayerTakeDamage.Last().EventTimeStamp.ToDateTimeFormat();

            if (killEventTimeStamp > killEventlastTimeStamp)
            {
                Func<Task> persistDataToMongo = async () => _genericRepository.Insert(logPlayerTakeDamage, "TakeDamage");

                await Task.Run(persistDataToMongo);

                //_genericRepository.Insert(logPlayerTakeDamage, "TakeDamage");

                killEventlastTimeStamp = killEventTimeStamp;
            }

        }

        private IEnumerable<TakeDamage> GetLogPlayerTakeDamage(JArray jsonToJObject)
        {   
            var result = jsonToJObject.Where(x => x.Value<string>("_T") == "LogPlayerTakeDamage").Select(s => new TakeDamage()
            {
                AttackId = s.Value<int>("attackId"),
                Attacker = new Attacker()
                {
                    Name = (string)s["attacker"]["name"],
                    TeamId = (int)s["attacker"]["teamId"],
                    Health = (float)s["attacker"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["attacker"]["location"]["x"],
                        y = (float)s["attacker"]["location"]["y"],
                        z = (float)s["attacker"]["location"]["z"],
                    },
                    Ranking = (int)s["attacker"]["ranking"],
                    AccountId = (string)s["attacker"]["accountId"]
                },
                Victim = new Victim()
                {
                    Name = (string)s["victim"]["name"],
                    TeamId = (int)s["victim"]["teamId"],
                    Health = (float)s["victim"]["health"],
                    Location = new Location()
                    {
                        x = (float)s["victim"]["location"]["x"],
                        y = (float)s["victim"]["location"]["y"],
                        z = (float)s["victim"]["location"]["z"],
                    },
                    Ranking = (int)s["victim"]["ranking"],
                    AccountId = (string)s["victim"]["accountId"]
                },
                DamageTypeCategory = (string)s["damageTypeCategory"],
                DamageReason = (string)s["damageReason"],
                Damage = (int)s["damage"],
                DamageCauserName = (string)s["damageCauserName"],                
                Common = new Common()
                {
                    MatchId = (string)s["common"]["matchId"],
                    MapName = (string)s["common"]["mapName"],
                    IsGame = (float)s["common"]["isGame"]

                },
                Version = (int)s["_V"],
                EventTimeStamp = (string)s["_D"],
                EventType = (string)s["_T"]

            });

            return result;
        }
    }
}
