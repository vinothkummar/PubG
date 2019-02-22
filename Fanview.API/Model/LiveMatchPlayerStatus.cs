﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class LiveMatchPlayerStatus
    {
        [BsonElement("playerId")]
        public int PlayerId { get; set; }
        [BsonElement("playerName")]
        public string PlayerName { get; set; }
        [BsonElement("isAlive")]
        public bool IsAlive { get; set; }
        [BsonElement("location")]
        public Location Location { get; set; }
        [BsonElement("health")]
        public float Health { get; set; }
        [BsonElement("boostGauge")]
        public int BoostGauge { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("armedWeapon")]
        public string ArmedWeapon { get; set; }
        [BsonElement("armedAmmoCount")]
        public int ArmedAmmoCount { get; set; }
        [BsonElement("inventoryAmmoCount")]
        public int InventoryAmmoCount { get; set; }

    }
}
