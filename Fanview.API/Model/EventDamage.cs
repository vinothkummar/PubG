using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class EventDamage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("matchId")]
        public string MatchId { get; set; }

        [BsonElement("damage")]
        public double Damage { get; set; }

        [BsonElement("isDetailStatus")]
        public bool IsDetailStatus { get; set; }

        [BsonElement("isVictimMe")]
        public bool IsVictimMe { get; set; }

        [BsonElement("damageTypeCategory")]
        public string DamageCategory { get; set; }

        [BsonElement("attackerName")]
        public string AttackerName { get; set; }

        [BsonElement("attackerLocation")]
        public Location AttackerLocation { get; set; }

        [BsonElement("attackerTeamId")]
        public int AttackerTeamId { get; set; }

        [BsonElement("victimName")]
        public string VictimName { get; set; }

        [BsonElement("victimLocation")]
        public Location VictimLocation { get; set; }

        [BsonElement("victimTeamId")]
        public int VictimTeamId { get; set; }

        [BsonElement("eventTimeStamp")]
        public double EventTimeStamp { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("eventSourceFileName")]
        public string EventSourceFileName { get; set; }
    }
}
