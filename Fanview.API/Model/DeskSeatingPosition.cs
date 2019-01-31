using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class DeskSeatingPosition
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("deskNumber")]
        public int DeskNumber { get; set; }
        [BsonElement("teamIdShort")]
        public int TeamIdShort { get; set; }
        [BsonElement("seat1PlayerId")]
        public int Seat1PlayerId { get; set; }
        [BsonElement("seat2PlayerId")]
        public int Seat2PlayerId { get; set; }
        [BsonElement("seat3PlayerId")]
        public int Seat3PlayerId { get; set; }
        [BsonElement("seat4PlayerId")]
        public int Seat4PlayerId { get; set; }
    }
}
