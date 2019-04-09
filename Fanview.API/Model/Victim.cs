using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class Victim
    {
        
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("health")]
        public double Health { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("ranking")]
        public int Ranking { get; set; }
        [BsonElement("accountId")]
        public string AccountId { get; set; }
    }
}