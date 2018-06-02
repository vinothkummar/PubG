using MongoDB.Bson.Serialization.Attributes;

namespace FanviewPollingService.Model
{
    public class Victim
    {
        
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
    }
}