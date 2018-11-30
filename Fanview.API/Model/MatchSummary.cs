using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class MatchSummary
    {
        [BsonId]
        [BsonElement("id")]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }        
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("attributes")]
        public MatchAttributes Attributes { get; set; }
        [BsonElement("relationsShips")]
        public MatchRelationships relationships { get; set; }
        [BsonElement("matchParticipant")]
        public IEnumerable<MatchParticipant> MatchParticipant { get; set; }
        [BsonElement("matchRoster")]
        public IEnumerable<MatchRoster> MatchRoster { get; set; }
    }
}
