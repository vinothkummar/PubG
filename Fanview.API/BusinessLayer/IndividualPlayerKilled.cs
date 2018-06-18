using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Utility;

namespace Fanview.API.BusinessLayer
{
    public class IndividualPlayerKilled : IKillingRule
    {
        public IEnumerable<string> PlayerKilledOrTeamEliminiation(IEnumerable<Kill> playerKilled)
        {
            var result = playerKilled                                     
                                     .Select(s => new
                                     {
                                         TimeKilled = s.EventTimeStamp,
                                         KillerName = s.Killer.Name,
                                         VictimName = s.Victim.Name,
                                         DamageCause = s.DamageCauserName,
                                        // DamageReason = s.DamageReason,
                                         VictimTeamId = s.Victim.TeamId,
                                         KillerTeamId = s.Killer.TeamId,
                                         VictimHealth = s.Victim.Health,
                                     });            

            var killMessages = new List<string>();

            var teamCount = new List<int>();

            string killText;

            foreach (var item in result)
            {                
                var playerLeftCount = FindPlayerLeft(playerKilled, killMessages);

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                    killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}  " +
                    $"{item.KillerName.ToUpper()} {item.KillerTeamId} KILLED  {item.VictimName.ToUpper()} " + //BY {item.DamageReason}
                    $"{item.VictimTeamId} WITH {ReadAssets.GetDamageCauserName(item.DamageCause).ToUpper()}   " +
                    $"{playerLeft}";
                 
                killMessages.Add(killText);

                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playerKilled.Where(cn => cn.Victim.TeamId == item.VictimTeamId).Count();


                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamLeftCount = FindTeamLeft(playerKilled, killMessages);

                    var teamLeft = teamLeftCount == 1 ? "winner" : teamLeftCount.ToString() + " LEFT";

                    killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")} " +
                               $" TEAM {item.VictimTeamId} HAS BEEN ELIMINATED " +
                               $"{teamLeft}";

                    killMessages.Add(killText);
                }

            }

            return killMessages;

        }

        private int FindPlayerLeft(IEnumerable<Kill> playerKilled, List<string> killMessages)
        {
            var teamEliminatedMessageCount = killMessages.Where(cn => cn.Contains("TEAM")).Count();

            var  lostHealthCount = playerKilled.Where(cn => cn.Victim.Health == 0).Count();

            return (lostHealthCount - killMessages.Count() ) + teamEliminatedMessageCount;
        }

        private int FindTeamLeft(IEnumerable<Kill> playerKilled, List<string> killMessages)
        {
            var playerKilledMessageCount = killMessages.Where(cn => cn.Contains("KILLED")).Count();

            var TeamCount = playerKilled.GroupBy(x => x.Victim.TeamId).Count();

            var teamEliminatedCount = killMessages.Where(cn => cn.Contains("TEAM")).Count() != 0 ? 
                                      killMessages.Where(cn => cn.Contains("TEAM")).Count() + 1 : 1;

            return (TeamCount - teamEliminatedCount);
        }
    }
}
