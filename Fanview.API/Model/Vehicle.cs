using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class Vehicle
    {        
        [BsonElement("vehicleType")]
        public string VehicleType { get; set; }
        [BsonElement("vehicleId")]
        public string VehicleId  { get; set; }
        [BsonElement("healthPercent")]
        public float HealthPercent { get; set; }
        [BsonElement("feulPercent")]
        public float FeulPercent { get; set; }       
    }
}
