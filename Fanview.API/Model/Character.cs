using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class Character
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("health")]
        public double Health { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("ranking")]
        public int Ranking { get; set; }
        [BsonElement("accountId")]
        public string AccountId { get; set; }
        [BsonElement("isInBlueZone")]
        public Boolean IsInBlueZone { get; set; }
        [BsonElement("isInRedZone")]
        public Boolean isInRedZone { get; set; }
    }
}
