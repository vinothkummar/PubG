using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class EventLiveMatchStatus
    {
        [BsonId]

        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        [BsonElement("isDetailStatus")]
        public bool IsDetailStatus { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("teamMode")]
        public string TeamMode{ get; set; }
        [BsonElement("cameraMode")]
        public string CameraMode { get; set; }
        [BsonElement("matchState")]
        public string MatchState { get; set; }
        [BsonElement("elapsedTime")]
        public int ElapsedTime { get; set; }
        [BsonElement("blueZonePhase")]
        public int BlueZonePhase { get; set; }
        [BsonElement("isBlueZoneMoving")]
        public bool IsBlueZoneMoving { get; set; }
        [BsonElement("blueZoneRadius")]
        public int BlueZoneRadius { get; set; }
        [BsonElement("blueZoneLocation")]
        public Location BlueZoneLocation { get; set; }
        [BsonElement("whiteZoneRadius")]
        public int WhiteZoneRadius { get; set; }
        [BsonElement("whiteZoneLocation")]
        public Location WhiteZoneLocation { get; set; }
        [BsonElement("redZoneRadius")]
        public int RedZoneRadius { get; set; }
        [BsonElement("redZoneLocation")]
        public Location RedZoneLocation { get; set; }
        [BsonElement("startPlayerCount")]
        public int StartPlayerCount { get; set; }
        [BsonElement("alivePlayerCount")]
        public int AlivePlayerCount { get; set; }
        [BsonElement("startTeamCount")]
        public int StartTeamCount { get; set; }
        [BsonElement("aliveTeamCount")]
        public int AliveTeamCount { get; set; }
        [BsonElement("playerInfos")]
        public List<EventMatchStatusPlayerInfo> PlayerInfos { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }
        [BsonElement("eventSourceFileName")]
        public string EventSourceFileName { get; set; }
    }
}
