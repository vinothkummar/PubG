using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Repository.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository
{
    public class LiveRepository : ILiveRepository
    {
        
        private IGenericRepository<EventDamage> _genericDamageRepository;
        private IGenericRepository<Team> _team;
        private IGenericRepository<TeamPlayer> _teamPlayers;

        public LiveRepository(IGenericRepository<Team> team,
                              IGenericRepository<TeamPlayer> teamPlayers, IGenericRepository<EventDamage> genericDamageRepository)
        {
            _genericDamageRepository = genericDamageRepository;
            _team = team;
            _teamPlayers = teamPlayers;         
        }

        public async Task<LiveDamageList> GetLiveDamageList(string matchId)
        {
            var teams =  await _team.GetAll("Team");

            var teamPlayers = await _teamPlayers.GetAll("TeamPlayers");
            var eventDamage = await _genericDamageRepository.GetAll("TeamPlayers");


            var response = eventDamage.GroupBy(o => o.VictimName)
                           .Select(ed => new DamageList
                           {
                               PlayerName = ed.Key,
                               TeamId = ed.FirstOrDefault().VictimTeamId,
                               PlayerId = teamPlayers.FirstOrDefault(o => o.PlayerName == ed.Key).PlayerId,
                               TeamName = teams.FirstOrDefault(o => o.TeamId == ed.FirstOrDefault().VictimTeamId).Name,
                               DamageDealt = ed.Sum(s => s.Damage)
                           }).OrderByDescending(ob => ob.DamageDealt).Take(10);

            return new LiveDamageList() { DamageList = response };
        }
    }
}
