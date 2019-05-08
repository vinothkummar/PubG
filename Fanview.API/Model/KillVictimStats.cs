using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model.LiveModels.LiveEvents
{
    public class KillVictimStats
    {
        [BsonElement("killCount")]
        public int killCount { get; set; }
        [BsonElement("score")]
        public int score { get; set; }
        [BsonElement("xp")]
        public int xp { get; set; }
        [BsonElement("distanceOnFoot")]
        public float distanceOnFoot { get; set; }
        [BsonElement("distanceOnSwim")]
        public double distanceOnSwim { get; set; }
        [BsonElement("distanceOnVehicle")]
        public float distanceOnVehicle { get; set; }
        [BsonElement("distanceOnParachute")]
        public float distanceOnParachute { get; set; }
        [BsonElement("distanceOnFreefall")]
        public float distanceOnFreefall { get; set; }
    }
}