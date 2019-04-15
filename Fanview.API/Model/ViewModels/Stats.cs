using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model.ViewModels
{
    public class Stats
    {
        [BsonElement("Knocks")]
        public double Knocks { get; set; }
        [BsonElement("Assists")]
        public double Assists { get; set; }
        [BsonElement("Boosts")]
        public double Boosts { get; set; }
        [BsonElement("damage")]
        public double damage{ get; set; }
        [BsonElement("headShot")]
        public double headShot { get; set; }
        [BsonElement("Heals")]
        public double Heals { get; set; }
        [BsonElement("Kills")]
        public double Kills { get; set; }
        [BsonElement("TimeSurvived")]
        public double TimeSurvived { get; set; }
        [BsonElement("Revives")]
        public double Revives { get; set; }
        [BsonElement("RideDistance")]
        public double RideDistance { get; set; }
        [BsonElement("SwimDistance")]
        public double SwimDistance { get; set; }
        [BsonElement("WalkDistance")]
        public double WalkDistance { get; set; }
        
        
    }
}
