using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class PlayerProfileTournament
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("MatchId")]
        public string  MatchId { get; set; }
        [BsonElement("PlayerId")]
        public int PlayerId { get; set; }
        [BsonElement("PlayerName")]
        public string PlayerName { get; set; }
        [BsonElement("FullName")]
        public string FullName { get; set; }
        [BsonElement("Country")]
        public string Country { get; set; }
        [BsonElement("TeamId")]
        public int TeamId { get; set; }
        [BsonElement("NumMatches")]
        public int NumMatches { get; set; }
        [BsonElement("stats")]
        public Stats stats { get; set; }
       

    }
}
