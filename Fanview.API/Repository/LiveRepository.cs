using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Fanview.API.Model.LiveModels;
using Fanview.API.GraphicsDummyData;
using Fanview.API.Model;

namespace Fanview.API.Repository
{
    public class LiveRepository : ILiveRepository
    {
        private LiveGraphichsDummyData _data;
        private IGenericRepository<EventDamage> _genericDamageRepository;
        private IGenericRepository<Team> _team;
        private IGenericRepository<TeamPlayer> _teamPlayers;

        public LiveRepository(IGenericRepository<Team> team,
                              IGenericRepository<TeamPlayer> teamPlayers, IGenericRepository<EventDamage> genericDamageRepository)
        {
            _genericDamageRepository = genericDamageRepository;
            _team = team;
            _teamPlayers = teamPlayers;
            _data = new LiveGraphichsDummyData();
        }
       
        
        public Task<LiveStatus> GetLiveStatus(string matchId)
        {
            return Task.FromResult(_data.GetDummyLiveStatus());
        }

        public async Task<LiveDamageList> GetLiveDamageList(string matchId)
        {
            var teams = _team.GetAll("Team").Result;

            var teamPlayers = _teamPlayers.GetAll("TeamPlayers").Result;
            var eventDamage = _genericDamageRepository.GetAll("TeamPlayers").Result;


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

        public Task<LiveKillList> GetLiveKillList(string matchId)
        {
            return Task.FromResult(_data.GetLiveKillList());
        }

        public Task<LivePlayerStats> GetLivePlayerStats(string matchId)
        {
            return Task.FromResult(_data.GetDummyLiveplayerstats());
        }
    }
}
