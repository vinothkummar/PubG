using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Model
{
    public class ParticipantStats
    {
        [BsonElement("DBNOs")]
        public int DBNOs { get; set; }
        [BsonElement("assists")]
        public int Assists { get; set; }
        [BsonElement("boosts")]
        public int Boosts { get; set; }
        [BsonElement("damageDealt")]
        public float DamageDealt { get; set; }
        [BsonElement("deathType")]
        public string DeathType { get; set; }
        [BsonElement("headshotKills")]
        public int HeadshotKills { get; set; }
        [BsonElement("heals")]
        public int Heals { get; set; }
        [BsonElement("killPlace")]
        public int KillPlace { get; set; }
        [BsonElement("killPoints")]
        public int KillPoints { get; set; }
        [BsonElement("killPointsDelta")]
        public float KillPointsDelta { get; set; }
        [BsonElement("killStreaks")]
        public int KillStreaks { get; set; }
        [BsonElement("kills")]
        public int Kills { get; set; }
        [BsonElement("lastKillPoints")]
        public int LastKillPoints { get; set; }
        [BsonElement("lastWinPoints")]
        public int LastWinPoints { get; set; }
        [BsonElement("longestKill")]
        public int LongestKill { get; set; }
        [BsonElement("mostDamage")]
        public int MostDamage { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("playerId")]
        public string PlayerId { get; set; }
        [BsonElement("revives")]
        public int Revives { get; set; }
        [BsonElement("rideDistance")]
        public float RideDistance { get; set; }
        [BsonElement("roadKills")]
        public int RoadKills { get; set; }
        [BsonElement("swimDistance")]
        public float SwimDistance { get; set; }
        [BsonElement("teamKills")]
        public int TeamKills { get; set; }
        [BsonElement("timeSurvived")]
        public int TimeSurvived  { get; set; }
        [BsonElement("vechicleDestroys")]
        public int VechicleDestroys { get; set; }
        [BsonElement("walkDistance")]
        public float WalkDistance { get; set; }
        [BsonElement("weaponsAcquired")]
        public int WeaponsAcquired { get; set; }
        [BsonElement("winPlace")]
        public int WinPlace { get; set; }
        [BsonElement("winPoints")]
        public float WinPoints  { get; set; }
        [BsonElement("winPointsDelta")]
        public float WinPointsDelta { get; set; }
    }
}
