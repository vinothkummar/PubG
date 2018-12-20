using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveMatchStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("teamName")]
        public string  TeamName { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("teamPlayers")] 
        public IEnumerable<LiveMatchPlayerStatus> TeamPlayers { get; set; }
        [BsonElement("aliveCount")]
        public int AliveCount { get; set; }
        [BsonElement("deadCount")]
        public int DeadCount { get; set; }
        [BsonElement("isEliminated")]
        public bool IsEliminated { get; set; }
        [BsonElement("eliminatedAt")]
        public string EliminatedAt { get; set; }
    }
}