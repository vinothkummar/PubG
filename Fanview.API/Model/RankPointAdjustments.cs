using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class RankPointAdjustments
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("rankPointsAdjustments")]
        public string RankPointsAdjustments { get; set; }      
    }
}
