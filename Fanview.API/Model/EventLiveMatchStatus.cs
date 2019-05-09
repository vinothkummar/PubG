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
        [BsonElement("eventTimeStamp")]
        public double EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }
    }
}