using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Utility;
using Fanview.API.Repository.Interface;

namespace Fanview.API.BusinessLayer
{
    public class IndividualPlayerKilled : IKillingRule
    {  
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _teamPlayerRepository;
        private IAssetsRepository _assetsRepository;

        public IndividualPlayerKilled(ITeamRepository teamRepository, IAssetsRepository assetsRepository, ITeamPlayerRepository teamPlayerRepository)
        {            
            
            _teamRepository = teamRepository;
            _teamPlayerRepository = teamPlayerRepository;
            _assetsRepository = assetsRepository;
        }
       

        public IEnumerable<string> PlayerKilledOrTeamEliminiatedText(IEnumerable<Kill> playerKilled)
        {
            var result = playerKilled                                     
                                     .Select(s => new
                                     {
                                         TimeKilled = s.EventTimeStamp,
                                         KillerName = s.Killer.Name,
                                         VictimName = s.Victim.Name,
                                         DamageCause = s.DamageCauserName,
                                         VictimTeamId = s.Victim.TeamId,
                                         KillerTeamId = s.Killer.TeamId,
                                         VictimHealth = s.Victim.Health,
                                     });            

            var killMessages = new List<string>();

            var teamCount = new List<int>();

            string killText;

            foreach (var item in result)
            {                
                var playerLeftCount = FindPlayerLeftText(playerKilled, killMessages);

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                    killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}  " +
                    $"{item.KillerName.ToUpper()} KILLED  {item.VictimName.ToUpper()} " + //BY {item.DamageReason}
                    $"WITH {_assetsRepository.GetDamageCauserName(item.DamageCause).ToUpper()} " +
                    $"{playerLeft}";
                 
                killMessages.Add(killText);

                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playerKilled.Where(cn => cn.Victim.TeamId == item.VictimTeamId).Count();


                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamLeftCount = FindTeamLeftText(playerKilled, killMessages);

                    var teamLeft = teamLeftCount == 0 ? "winner" : teamLeftCount.ToString() + " LEFT";

                    killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")} " +
                               $" TEAM {item.VictimTeamId} HAS BEEN ELIMINATED " +
                               $"{teamLeft}";

                    killMessages.Add(killText);
                }

            }

            return killMessages;

        }

        private int FindPlayerLeftText(IEnumerable<Kill> playerKilled, List<string> killMessages)
        {
            var teamEliminatedMessageCount = killMessages.Where(cn => cn.Contains("TEAM")).Count();

            var  lostHealthCount = playerKilled.Where(cn => cn.Victim.Health == 0).Count();

            return (lostHealthCount - killMessages.Count() ) + teamEliminatedMessageCount;
        }

        private int FindTeamLeftText(IEnumerable<Kill> playerKilled, List<string> killMessages)
        {
            var playerKilledMessageCount = killMessages.Where(cn => cn.Contains("KILLED")).Count();

            var TeamCount = playerKilled.GroupBy(x => x.Victim.TeamId).Count();

            var teamEliminatedCount = killMessages.Where(cn => cn.Contains("TEAM")).Count() != 0 ? 
                                      killMessages.Where(cn => cn.Contains("TEAM")).Count() + 1 : 1;

            return (TeamCount - teamEliminatedCount);
        }

       

        public async Task<IEnumerable<KilliPrinter>> LiveKilledOrTeamEliminiated(IEnumerable<LiveEventKill> playerKilled)
        { 

            var result = await _teamPlayerRepository.GetPlayersId(playerKilled).ConfigureAwait(false);

            var killiPrinter = new List<KilliPrinter>();

            foreach (var item in result)
            {
                var playerKillMessage = new PlayerKilledGraphics()
                {
                    TimeKilled = item.TimeKilled,
                    KillerName = item.KillerName,
                    VictimName = item.VictimName,
                    VictimLocation = item.VictimLocation,
                    DamagedCausedBy = _assetsRepository.GetDamageCauserName(item.DamagedCausedBy),
                    DamageReason = item.DamageReason,
                    VictimTeamId = item.VictimTeamId,
                    KillerTeamId = item.KillerTeamId,
                    KillerPlayerId = item.KillerPlayerId,
                    VictimPlayerId = item.VictimPlayerId,                   
                };

                var killMessage = new KilliPrinter() { PlayerKilled = playerKillMessage };


                killiPrinter.Add(killMessage);
            }

            return killiPrinter;
        }
    }
}
