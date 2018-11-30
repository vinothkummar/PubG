using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchSummaryData
    {
        [BsonId]
        [BsonElement("id")]       
        public string Id { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("attributes")]
        public MatchAttributes Attributes { get; set; }
    }
}
