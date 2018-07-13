using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanview.API.Model.LiveModels;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerKillRepository
    {
        void InsertPlayerKillTelemetry(string jsonResult, string matchId);

        void PollTelemetryPlayerKilled(string jsonResult);

        Task<IEnumerable<Kill>> GetPlayerKilled(string matchId);

        Task<IEnumerable<Kill>> GetPlayerKilled(string matchId1, string matchId2, string matchId3, string matchId4);

        Task<IEnumerable<Kill>> GetLast4PlayerKilled(string matchId);

        Task<KillLeaderList> GetKillLeaderList(string matchId);

        Task<IEnumerable<KillZone>> GetKillZone(string matchId);
    }
}
