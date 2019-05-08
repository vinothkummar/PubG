using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model.LiveModels.LiveEvents
{
    public class Victimgameresult
    {
        [BsonElement("accountId")]
        public string accountId { get; set; }
        [BsonElement("rank")]
        public int rank { get; set; }
        [BsonElement("gameResult")]
        public string gameResult { get; set; }
        [BsonElement("teamId")]
        public int teamId { get; set; }
        [BsonElement("Stats")]
        public KillVictimStats KillVictimStats { get; set; }
    }
}