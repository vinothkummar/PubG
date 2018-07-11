using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchRanking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public int KillPoints { get; set; }
        public int RankPoints { get; set; }
        public int TotalPoints { get; set; }
        public string TeamId { get; set; }
        public string MatchId { get; set; }
        public int PubGOpenApiTeamId { get; set; }
    }
}
