using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class AssetsData
    {
        [BsonElement("data")]
        public IEnumerable<Assets> Data { get; set; }
    }
}
