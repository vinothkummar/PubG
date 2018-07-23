using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class GameState
    {
        [BsonElement("elapsedTime")]
        public int ElapsedTime { get; set; }
        [BsonElement("numAliveTeams")]
        public int NumAliveTeams { get; set; }
        [BsonElement("numJoinPlayers")]
        public int NumJoinPlayers { get; set; }
        [BsonElement("numStartPlayers")]
        public int NumStartPlayers { get; set; }
        [BsonElement("numAlivePlayers")]
        public int NumAlivePlayers { get; set; }
        [BsonElement("safetyZonePosition")]
        public SafetyZonePosition SafetyZonePosition { get; set; }
        [BsonElement("safetyZoneRadius")]
        public float SafetyZoneRadius { get; set; }
        [BsonElement("poisonGasWarningPosition")]
        public PoisonGasWarningPosition PoisonGasWarningPosition { get; set; }
        [BsonElement("poisonGasWarningRadius")]
        public float PoisonGasWarningRadius { get; set; }
        [BsonElement("redZonePosition")]
        public RedZonePosition RedZonePosition { get; set; }
        [BsonElement("redZoneRadius")]
        public float RedZoneRadius { get; set; }
    }
}