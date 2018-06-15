using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class Location
    {
        [BsonElement("x")]
        public float x { get; set; }
        [BsonElement("y")]
        public float y { get; set; }
        [BsonElement("z")]
        public float z { get; set; }
    }
}