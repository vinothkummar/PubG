using MongoDB.Bson;
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
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("createdAt")]
        public string CreatedAT { get; set; }
        [BsonElement("eventName")]
        public string EventName { get; set; }

        //[BsonElement("relationsShips")]
        //public EventRelationships relationships { get; set; }
        //[BsonElement("eventsDate")]
        //public IEnumerable<EventsDate> EventsDate { get; set; }
    }
}
