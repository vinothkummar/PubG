﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class MatchParticipant
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("participantAttributes")]
        public ParticipantAttributes ParticipantAttributes { get; set; }
    }
}
