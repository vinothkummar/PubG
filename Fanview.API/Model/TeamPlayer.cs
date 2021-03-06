﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    [BsonDiscriminator(RootClass = true)]
    public class TeamPlayer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("matchId")]
        public string MatchId { get; set; }
        [BsonElement("teamId")]
        public string TeamId { get; set; }
        [BsonElement("teamIdShort")]
        public int TeamIdShort { get; set; }
        [BsonElement("playerId")]
        public int PlayerId { get; set; }
        [BsonElement("playerName")]
        public string PlayerName { get; set; }
        [BsonElement("fullName")]
        public string FullName { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }
        [BsonElement("pubgAccountId")]
        public string PubgAccountId { get; set; }
        public Boolean PlayerStatus { get; set; }
        public Location Location { get; set; }
        public bool isduplicated { get; set; }
    }
}
