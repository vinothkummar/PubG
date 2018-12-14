using Fanview.API.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchRepository
    {
        //void PollMatchSessionId(string eventName);
        //void InsertEvent(string jsonResult, string eventName);

        void InsertMatchSummary(string jsonResult, string matchId);
        Task<string> GetMapName(string matchId);
        Task<JObject> GetMatchesDetailsByID(string id);
        Task<JObject> GetMatchIdByTournament(string tournament);
        Task<IEnumerable<Event>> GetTournamentMatchId();
        void InsertMatchSafeZonePosition(string jsonResult, string matchId);
        Task<Object> GetMatchSafeZone(int matchId);

        Task<IEnumerable<RankPoints>> GetMatchRankPoints();
        

    }
}
