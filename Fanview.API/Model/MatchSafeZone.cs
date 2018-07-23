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
        [BsonElement("common")]
        public MatchSafeZoneCommon MatchSafeZoneCommon { get; set; }
        [BsonElement("_D")]
        public string _D { get; set; }
        [BsonElement("_T")]
        public string _T { get; set; }
    }
}
