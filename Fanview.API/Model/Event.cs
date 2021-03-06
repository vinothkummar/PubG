﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class Event
    {
        [BsonId]
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public int MatchId { get; set; }
        [BsonElement("createdAt")]
        public string CreatedAT { get; set; }
        [BsonElement("eventName")]
        public string EventName { get; set; }
        [BsonElement("dataAvailable")]
        public bool DataAvailable  { get; set; }
    }
}
