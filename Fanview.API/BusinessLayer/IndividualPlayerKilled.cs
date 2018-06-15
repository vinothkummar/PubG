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
                                         VictimTeamId = s.Victim.TeamId,
                                         KillerTeamId = s.Killer.TeamId,
                                         VictimHealth = s.Victim.Health,
                                     });            

            var killMessages = new List<string>();

            foreach (var item in result)
            {
                var playerLeftCount = FindPlayerLeft(playerKilled, killMessages);

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                var killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}  " +
                    $"{item.KillerName.ToUpper()} {item.KillerTeamId} KILLED  {item.VictimName.ToUpper()} " +
                    $"{item.VictimTeamId} WITH {ReadAssets.GetDamageCauserName(item.DamageCause).ToUpper()}   " +
                    $"{playerLeft}";
                 
                killMessages.Add(killText);

            }

            return killMessages;

        }

        private static int FindPlayerLeft(IEnumerable<Kill> playerKilled, List<string> killMessages)
        {
          var  lostHealthCount = playerKilled.OrderByDescending(o => o.EventTimeStamp)
                                                .Where(cn => cn.Victim.Health == 0).Count();

            return lostHealthCount - killMessages.Count();
        }
    }
}
