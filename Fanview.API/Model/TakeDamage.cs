using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class TakeDamage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("attackid")]
        public int AttackId { get; set; }
        [BsonElement("attacker")]
        public Attacker Attacker { get; set; }
        [BsonElement("victim")]
        public Victim Victim { get; set; }
        [BsonElement("damageTypeCategory")]
        public string DamageTypeCategory { get; set; }
        [BsonElement("damageReason")]
        public string DamageReason { get; set; }
        [BsonElement("damage")]
        public int Damage { get; set; }
        [BsonElement("damageCauserName")]
        public string DamageCauserName { get; set; }
        [BsonElement("common")]
        public Common Common { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }
    }
}
