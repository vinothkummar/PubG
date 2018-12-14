using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Repository.Interface
{
    public interface IEventRepository
    {
        Task<string> GetTournamentMatchId(int matchId);

        Task<string> GetEventCreatedAt(int matchId);

        Task<Event> FindEvent(string matchId);

        Task<int> GetTournamentMatchCount();

        void CreateAnEvent(Event newMatch);
    }
}
