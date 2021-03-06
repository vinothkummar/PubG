﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("shortName")]
        public string ShortName { get; set; }
        [BsonElement("region")]
        public string Region { get; set; }
      

    }
}
