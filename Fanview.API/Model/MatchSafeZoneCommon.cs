using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class MatchSafeZoneCommon
    {
        [BsonElement("isGame")]
        public float IsGame { get; set; }
    }
}