using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchRoster
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("rosterAttributes")]
        public RosterAttributes RosterAttributes { get; set; }
        [BsonElement("rosterRelationShips")]
        public RosterRelationShips RosterRelationShips { get; set; }

    }
}
