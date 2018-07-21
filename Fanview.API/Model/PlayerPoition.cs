using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class PlayerPoition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }              
        [BsonElement("name")]
        public string Name { get; set; }        
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("health")]
        public float Health { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("ranking")]
        public int Ranking { get; set; }
        [BsonElement("accountId")]
        public string AccountId { get; set; }      
        [BsonElement("elapsedTime")]
        public string elapsedTime { get; set; }
        [BsonElement("numAlivePlayers")]
        public int NumAlivePlayers { get; set; }
        [BsonElement("common")]
        public Common Common { get; set; }        
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }
    }
}
