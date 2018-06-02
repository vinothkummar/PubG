using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FanviewPollingService.Model
{
    public class Common
    {
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("mapName")]
        public string MapName { get; set; }
        [BsonElement("isGame")]
        public float IsGame { get; set; }
    }
}
