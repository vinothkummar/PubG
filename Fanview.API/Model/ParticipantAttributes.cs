using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class ParticipantAttributes
    {
        [BsonElement("stats")]
        public ParticipantStats stats { get; set; }
        [BsonElement("actor")]
        public string Actor { get; set; }
        [BsonElement("shardId")]
        public string ShardId { get; set; }
    }
}
