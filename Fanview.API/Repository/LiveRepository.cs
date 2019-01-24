using Fanview.API.Clients;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository.Interface;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class LiveRepository : ILiveRepository
    {
        private readonly IMongoCollection<EventDamage> _eventDamageCollection;
        private readonly IMongoCollection<TeamPlayer> _teamPlayerCollection;
        private readonly IMongoCollection<Team> _teamCollection;

        public LiveRepository(IMongoDbClient dbClient)
        {
            _eventDamageCollection = dbClient.Database.GetCollection<EventDamage>("LiveEventDamage");
            _teamPlayerCollection = dbClient.Database.GetCollection<TeamPlayer>("TeamPlayers");
            _teamCollection = dbClient.Database.GetCollection<Team>("Team");
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
    }
}
