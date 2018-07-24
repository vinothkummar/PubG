using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class MatchSafeZone
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; } 
        [BsonElement("gameState")]
        public GameState GameState { get; set; }
        [BsonElement("matchSafeZoneCommon")]
        public MatchSafeZoneCommon MatchSafeZoneCommon { get; set; }        
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }       
    }
}
