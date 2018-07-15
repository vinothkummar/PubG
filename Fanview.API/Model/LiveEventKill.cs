using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveEventKill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("isKillerMe")]
        public bool IsKillerMe { get; set; }
        [BsonElement("killerName")]
        public string KillerName { get; set; }
        [BsonElement("killerLocation")]
        public Location KillerLocation { get; set; }
        [BsonElement("killerTeamId")]
        public string KillerTeamId { get; set; }
        [BsonElement("isVictimMe")]
        public bool IsVictimMe { get; set; }
        [BsonElement("victimName")]
        public string VictimName { get; set; }
        [BsonElement("victimLocation")]
        public Location VictimLocation { get; set; }
        [BsonElement("victimTeamId")]
        public string VictimTeamId { get; set; }
        [BsonElement("damageCauser")]
        public string DamageCauser { get; set; }
        [BsonElement("damageReason")]
        public string DamageReason { get; set; }
        [BsonElement("isGroggy")]
        public bool IsGroggy { get; set; }
        [BsonElement("isStealKilled")]
        public bool IsStealKilled { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }

    }
}
