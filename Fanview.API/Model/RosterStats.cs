using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class RosterStats
    {
        [BsonElement("rank")]
        public int Rank { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
    }
}
