using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanview.API.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Kill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("attackid")]
        public int AttackId { get; set; }
        [BsonElement("killer")]
        public Killer Killer { get; set; }
        [BsonElement("victim")]
        public Victim Victim { get; set; }
        [BsonElement("damageTypeCategory")]
        public string DamageTypeCategory { get; set; }        
        [BsonElement("damageCauserName")]
        public string DamageCauserName { get; set; }
        [BsonElement("distance")]
        public float Distance { get; set; }
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
