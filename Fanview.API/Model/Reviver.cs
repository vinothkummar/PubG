using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model.LiveModels.LiveEvents
{
    public class Reviver
    {
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("teamId")]
        public int teamId { get; set; }
        [BsonElement("health")]
        public float health { get; set; }
        [BsonElement("location")]
        public Location location { get; set; }
        [BsonElement("ranking")]
        public int ranking { get; set; }
        [BsonElement("accountid")]
        public string accountId { get; set; }
    }
}