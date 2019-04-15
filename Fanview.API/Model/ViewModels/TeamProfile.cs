using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class TeamProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("TeamId")]
        public int TeamId { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Region")]
        public string Region { get; set; }
        [BsonElement("MatchNum")]
        public int MatchNum { get; set; }
        [BsonElement("stats")]
        public Stats stats { get; set; }
        [BsonElement("ShortName")]
        public string ShortName { get; set; }
    }
}
