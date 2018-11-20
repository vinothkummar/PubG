using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveMatchPlayerStatus
    {
        [BsonElement("playerId")]
        public int PlayerId { get; set; }
        [BsonElement("playerName")]
        public string PlayerName { get; set; }
        [BsonElement("isAlive")]
        public bool IsAlive { get; set; }

    }
}
