using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class EventsAttributes
    {
        [BsonElement("createdAt")]
        public string CreatedAT { get; set; }
    }
}
