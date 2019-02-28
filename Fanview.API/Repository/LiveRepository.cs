using Fanview.API.Clients;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository.Interface;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class LiveRepository : ILiveRepository
    {
        private IGenericRepository<LiveEventKill> _genericEventKillRepository;
        private IGenericRepository<EventLiveMatchStatus> _genericEventlivestateRepository;
        private IGenericRepository<EventDamage> _genericLiveDamage;

        private IGenericRepository<LiveMatchStatus> _genericmatchstates;
        private readonly IMongoCollection<EventDamage> _eventDamageCollection;
        private readonly IMongoCollection<TeamPlayer> _teamPlayerCollection;
        private readonly IMongoCollection<Team> _teamCollection;

        public LiveRepository(IMongoDbClient dbClient, IGenericRepository<LiveEventKill> genericEventKillRepository, 
            IGenericRepository<LiveMatchStatus> genericmatchstates,IGenericRepository<EventLiveMatchStatus> genericEventlivestateRepository,
            IGenericRepository<EventDamage> genericLiveDamage)

        {
            _eventDamageCollection = dbClient.Database.GetCollection<EventDamage>("LiveEventDamage");
            _teamPlayerCollection = dbClient.Database.GetCollection<TeamPlayer>("TeamPlayers");
            _teamCollection = dbClient.Database.GetCollection<Team>("Team");
            _genericEventKillRepository = genericEventKillRepository;
            _genericmatchstates = genericmatchstates;
            _genericEventlivestateRepository = genericEventlivestateRepository;
            _genericLiveDamage = genericLiveDamage;
        }

        public async Task<LiveDamageList> GetLiveDamageList()
        {
            var eventDamageFilter = Builders<EventDamage>.Filter.Empty;
            var eventDamageQuery = await _eventDamageCollection.FindAsync(eventDamageFilter).ConfigureAwait(false);
            var eventDamage = await eventDamageQuery.ToListAsync().ConfigureAwait(false);
            var teamPlayerQueryable = _teamPlayerCollection.AsQueryable();
            var teamQueryable = _teamCollection.AsQueryable();

            var result = await eventDamage
                .Join(teamPlayerQueryable, ed => ed.VictimName, tp => tp.PlayerName, (ed, tp) => new { ed, tp })
                .Join(teamQueryable, edTp => edTp.tp.TeamId, t => t.Id, (edTp, t) => new { edTp, t })
                .GroupBy(edTpt => edTpt.edTp.tp.PlayerName)
                .Select(edTpt => new DamageList
                {
                    PlayerName = edTpt.Key,
                    TeamId = edTpt.FirstOrDefault().edTp.ed.VictimTeamId,
                    PlayerId = edTpt.FirstOrDefault().edTp.tp.PlayerId,
                    TeamName = edTpt.FirstOrDefault().t.Name,
                    DamageDealt = edTpt.Sum(e => e.edTp.ed.Damage)
                })
                .ToAsyncEnumerable()
                .ToArray()
                .ConfigureAwait(false);

            return new LiveDamageList()
            {
                DamageList = result
            };
        }
        public void DeleteAllEventKillTable()
        {
            var filter = Builders<LiveEventKill>.Filter.Empty;
            _genericEventKillRepository.DeleteMany(filter, "LiveEventKill");
        }
        public void DeleteAllTeamStates()
        {
            var filter = Builders<LiveMatchStatus>.Filter.Empty;
            _genericmatchstates.DeleteMany(filter, "TeamLiveStatus");
            
        }
        public void DeleteEventMatchStates()
        {
            var filter = Builders<EventLiveMatchStatus>.Filter.Empty;
            _genericEventlivestateRepository.DeleteMany(filter, "EventMatchStatus");

        }
        public void DeleteEventLiveMatchDamage()
        {
            var filter = Builders<EventDamage>.Filter.Empty;
            _genericLiveDamage.DeleteMany(filter, "LiveEventDamage");

        }
        public void DeleteAll()
        {
            //collectionNames include 4 collections to be deleted inside.
            var CollectionNames = new List<string>() { "TeamLiveStatus", "LiveEventKill", "EventMatchStatus", "LiveEventDamage" };
            //loop through collections and delete each of 4 collection
            foreach (var item in CollectionNames)
            {
                var filter = Builders<EventDamage>.Filter.Empty;
                _genericLiveDamage.DeleteMany(filter, item);
            }

        }


    }
}
