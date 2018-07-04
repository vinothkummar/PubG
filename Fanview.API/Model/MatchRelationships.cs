using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchRelationships
    {
        [BsonElement("rosters")]
        public RostersData Rosters { get; set; }

        [BsonElement("assets")]
        public AssetsData Assets { get; set; }

    }
}
