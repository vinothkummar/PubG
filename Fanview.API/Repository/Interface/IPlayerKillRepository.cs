using Fanview.API.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanview.API.Model.ViewModels;
using Fanview.API.Model.LiveModels;
using Newtonsoft.Json.Linq;
using Fanview.API.Model.ViewModels;

namespace Fanview.API.Repository.Interface
{
    public interface IPlayerKillRepository
    {
        void InsertPlayerKillTelemetry(string jsonResult, string matchId);

        void PollTelemetryPlayerKilled(string jsonResult);

        Task<IEnumerable<Kill>> GetPlayerKilled(string matchId);

        Task<IEnumerable<LiveEventKill>> GetLiveKilled(int matchId);

        Task<IEnumerable<Kill>> GetPlayerKilled(string matchId1, string matchId2, string matchId3, string matchId4);

        Task<IEnumerable<Kill>> GetLast4PlayerKilled(string matchId);

        Task<KillLeader> GetKillLeaderList(int matchId, int topCount);

        Task<IEnumerable<KillZone>> GetKillZone(int matchId);

        void InsertLiveKillEventTelemetry(JObject[] jsonResult, string fileName);

        Task<KillLeader> GetKillLeaderList();

        Task<KillLeader> GetLiveKillList(string matchId);

        Task<IEnumerable<LiveKillCount>> GetLiveKillCount(IEnumerable<LiveEventKill> liveEventKills);
    }
}
