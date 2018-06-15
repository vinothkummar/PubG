﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class Attacker
    {

        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("teamId")]
        public int TeamId { get; set; }
        [BsonElement("health")]
        public float Health { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("ranking")]
        public int Ranking { get; set; }
        [BsonElement("accountId")]
        public string AccountId { get; set; }
    }
}
