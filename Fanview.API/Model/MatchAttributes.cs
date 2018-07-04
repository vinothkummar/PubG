using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchAttributes
    {
        [BsonElement("stats")]
        public string Stats { get; set; }
        [BsonElement("gameMode")]
        public string GameMode { get; set; }
        [BsonElement("tags")]
        public string Tags { get; set; }
        [BsonElement("mapName")]
        public string MapName { get; set; }
        [BsonElement("isCustomMatch")]
        public bool IsCustomeMatch { get; set; }
        [BsonElement("createdAt")]
        public string CreatedAT{ get; set; }
        [BsonElement("duration")]
        public int Duration { get; set; }
        [BsonElement("titleId")]
        public string TitleId { get; set; }
        [BsonElement("shardId")]
        public string ShardId { get; set; }
    }
}
