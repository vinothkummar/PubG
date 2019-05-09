using Fanview.API.Model.LiveModels.LiveEvents;
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
        [BsonElement("teamSize")]
        public int teamSize { get; set; }
        [BsonElement("blueZoneCustomOptions")]
        public string BlueZoneCustomOptions { get; set; }
        [BsonElement("isCustomGame")]
        public bool IsCustomGame { get; set; }
        [BsonElement("mapName")]
        public string MapName { get; set; }
        [BsonElement("weatherId")]
        public string WeatherId { get; set; }
        [BsonElement("characters")]
        public Character Characters { get; set; }
        [BsonElement("cameraViewBehaviour")]
        public string CameraViewBehaviour { get; set; }
        [BsonElement("isEventMode")]
        public bool IsEventMode { get; set; }
        [BsonElement("_V")]
        public int _V { get; set; }
        [BsonElement("_D")]
        public DateTime _D { get; set; }
        [BsonElement("_T")]
        public string _T { get; set; }
        [BsonElement("_U")]
        public bool _U { get; set; }
        [BsonElement("attackId")]
        public int AttackId { get; set; }
        [BsonElement("attacker")]
        public Attacker Attacker { get; set; }
        [BsonElement("victim")]
        public Victim Victim { get; set; }
        [BsonElement("damageTypeCategory")]
        public string DamageTypeCategory { get; set; }
        [BsonElement("DamageReason ")]
        public string damageReason { get; set; }
        [BsonElement("Damage")]
        public float damage { get; set; }
        [BsonElement("damageCauserName")]
        public string DamageCauserName { get; set; }
        [BsonElement("damageCauserAdditionalInfo")]
        public List<string> DamageCauserAdditionalInfo { get; set; }
        [BsonElement("distance")]
        public float Distance { get; set; }
        [BsonElement("isAttackerInVehicle")]
        public bool IsAttackerInVehicle { get; set; }
        [BsonElement("dBNOId")]
        public int DBNOId { get; set; }
        [BsonElement("killer")]
        public Killer Killer { get; set; }
        [BsonElement("Assistant")]
        public Assistant assistant { get; set; }
        [BsonElement("VictimGameResult")]
        public Victimgameresult VictimGameResult { get; set; }
        [BsonElement("reviver")]
        public Reviver Reviver { get; set; }
    }
}