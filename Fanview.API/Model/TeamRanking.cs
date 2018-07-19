using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class TeamRanking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string TeamId { get; set; }
        public string TeamRank { get; set; }
        public string TeamName { get; set; }
        public int Kill{ get; set; }
        public float Damage { get; set; }
        public int TotalPoints { get; set; }        
        public string MatchId { get; set; }
    }
}
