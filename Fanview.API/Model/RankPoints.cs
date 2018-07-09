using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class RankPoints
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("tournamentName")]
        public string TournamentName { get; set; }
        [BsonElement("rankPosition")]
        public int RankPosition { get; set; }
        [BsonElement("scoringPoints")]
        public int ScoringPoints { get; set; }
    }
}
