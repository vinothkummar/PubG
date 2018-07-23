using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class SafetyZonePosition
    {
        [BsonElement("x")]
        public float X { get; set; }
        [BsonElement("y")]
        public float Y { get; set; }
        [BsonElement("z")]
        public float Z { get; set; }
    }
}