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
        public IEnumerable<string> PlayerKilledOrTeamEliminiation(IEnumerable<PlayerKill> playerKilled)
        {
           var result = playerKilled
                                    //.OrderByDescending(o => o.EventTimeStamp)
                                    .Select(s => new { TimeKilled = s.EventTimeStamp, KillerName = s.Killer.Name,
                                                       VictimName = s.Victim.Name, DamageCause = s.DamageCauserName,
                                                       DamageTypeCategroy = s.DamageTypeCategory,
                                                       VictimRanking = s.Victim.Ranking, VictimTeamId = s.Victim.TeamId,
                                                       KillerTeamId = s.Killer.TeamId, VictimHealth = s.Victim.Health });

            var victimHealthCount = playerKilled.OrderByDescending(o => o.EventTimeStamp)
                                                .Where(cn => cn.Victim.Health == 0).Count();

            var killMessages = new List<string>();

            

            foreach (var item in result)
            {
                var playerLeft = victimHealthCount - killMessages.Count();

                var killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("HH:mm")}  " +
                    $"{item.KillerName.ToUpper()} {item.KillerTeamId} KILLED  {item.VictimName.ToUpper()} " +
                    $"{item.VictimTeamId} WITH {ReadAssets.GetDamageCauserName(item.DamageCause).ToUpper()}   " +
                    $"{playerLeft} {item.VictimRanking} LEFT ";

                killMessages.Add(killText);

            }

            return killMessages;

        }
    }
}
