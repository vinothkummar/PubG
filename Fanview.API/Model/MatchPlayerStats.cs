using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fanview.API.Model
{
    public class MatchPlayerStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("rosterId")]
        public string RosterId { get; set; }
        [BsonElement("participantId")]
        public string ParticipantId { get; set; }
        [BsonElement("stats")]
        public ParticipantStats stats { get; set; }
        [BsonElement("teamId")]
        public string TeamId { get; set; }
        [BsonElement("rank")]
        public int Rank { get; set; }
        [BsonElement("shortTeamId")]
        public int ShortTeamId { get; set; }



    }
}
