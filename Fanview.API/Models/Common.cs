using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
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
