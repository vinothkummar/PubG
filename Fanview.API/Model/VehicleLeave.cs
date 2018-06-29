using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class VehicleLeave
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("character")]
        public Character Character { get; set; }
        [BsonElement("vehicle")]
        public Vehicle Vehicle { get; set; }
        [BsonElement("rideDistance")]
        public float RideDistance { get; set; }
        [BsonElement("seatIndex")]
        public int seatIndex { get; set; }
        [BsonElement("common")]
        public Common Common { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("eventTimeStamp")]
        public string EventTimeStamp { get; set; }
        [BsonElement("eventType")]
        public string EventType { get; set; }
    }
}
