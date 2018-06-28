using Fanview.API.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IMatchRepository
    {
        void PollMatchSessionId(string eventName);
        void InsertEvent(string jsonResult, string eventName);
        Task<JObject> GetMatchesDetailsByID(string id);
        Task<JObject> GetMatchIdByTournament(string tournament);
        Task<IEnumerable<Event>> GetTournamentMatchId();
    }
}
