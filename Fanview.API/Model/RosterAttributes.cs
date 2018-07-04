using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class RosterAttributes
    {
        [BsonElement("stats")]
        public RosterStats Stats { get; set; }
        [BsonElement("won")]
        public string Won { get; set; }
        [BsonElement("shardId")]
        public string shardId { get; set; }
       
    }
}
