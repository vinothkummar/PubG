using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class RostersData
    {
        [BsonElement("data")]
        public IEnumerable<Roster> Data { get; set; }
    }
}
