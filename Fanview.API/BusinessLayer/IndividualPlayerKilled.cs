using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Utility;
using Microsoft.Extensions.Logging;
using Fanview.API.Repository.Interface;

namespace Fanview.API.BusinessLayer
{
    public class IndividualPlayerKilled : IKillingRule
    {       
        private IReadAssets _readAssets;
        private ITeamRepository _teamRepository;

        public IndividualPlayerKilled(IReadAssets readAssets, ITeamRepository teamRepository)
        {            
            _readAssets = readAssets;
            _teamRepository = teamRepository;
        }
        public IEnumerable<KilliPrinter> PlayerKilledOrTeamEliminiated(IEnumerable<Kill> playerKilled)
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

            var killiPrinter = new List<KilliPrinter>();

            var teamCount = new List<int>();

            foreach (var item in result)
            {
                var playerLeftCount = FindPlayerLeft(playerKilled, killiPrinter);

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                var playerKillMessage = new PlayerKilledGraphics()
                {
                       TimeKilled = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}",
                       KillerName = $"{item.KillerName.ToUpper()}", 
                       FreeText1 = $"KILLED",
                       VictimName = $"{item.VictimName.ToUpper()}",
                       FreeText2 = $"WITH",
                       DamagedCausedBy = $"{_readAssets.GetDamageCauserName(item.DamageCause).ToUpper()} ",
                       PlayerLeft = $"{playerLeft}"
                };


                TeamEliminated teamEliminatedMessage = null;
                                
                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playerKilled.Where(cn => cn.Victim.TeamId == item.VictimTeamId).Count();


                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount)
                {
                    var teamLeftCount = FindTeamLeft(playerKilled, teamCount);

                    var teamLeft = teamLeftCount == 1 ? "winner" : teamLeftCount.ToString() + " LEFT";

                   teamEliminatedMessage = new TeamEliminated()
                    {
                        TimeElimnated = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}",
                        FreeText1 = $"Team",
                        TeamId = $"{item.VictimTeamId}",
                        FreeText2 =$"HAS BEEN ELIMINATED",
                        TeamLeft = $"{teamLeft}"
                    };
                }

                var killMessage = new KilliPrinter() {PlayerKilled = playerKillMessage, TeamEliminated = teamEliminatedMessage};


                killiPrinter.Add(killMessage);
            }

            return killiPrinter;


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
                var playerLeftCount = FindPlayerLeftText(playerKilled, killMessages);

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                    killText = $"{item.TimeKilled.ToDateTimeFormat().ToString("mm:ss")}  " +
                    $"{item.KillerName.ToUpper()} KILLED  {item.VictimName.ToUpper()} " + //BY {item.DamageReason}
                    $"WITH {_readAssets.GetDamageCauserName(item.DamageCause).ToUpper()} " +
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

        private int FindPlayerLeft(IEnumerable<Kill> playerKilled, List<KilliPrinter> killMessages)
        {
          

            var lostHealthCount = playerKilled.Where(cn => cn.Victim.Health == 0).Count();

            return (lostHealthCount - killMessages.Count()); //+ teamEliminatedMessageCount;
        }

        private int FindTeamLeft(IEnumerable<Kill> playerKilled, List<int> teamCount)
        {      
            var TeamCount = playerKilled.GroupBy(x => x.Victim.TeamId).Count();

            List<int> memberKeys = teamCount.GroupBy(x => x)
                            .Where(x => x.Count() == 4)
                            .Select(grp => grp.Key)
                            .ToList();

            return (TeamCount - memberKeys.Count);            
        }

        public IEnumerable<KilliPrinter> LiveKilledOrTeamEliminiated(IEnumerable<LiveEventKill> playerKilled, string CreateAt)
        {
            var result = playerKilled
                                       .Select(s => new
                                       {
                                           TimeKilled = s.EventTimeStamp,
                                           KillerName = s.KillerName,
                                           VictimName = s.VictimName,
                                           DamageCause = s.DamageCauser,
                                           s.DamageReason,
                                           VictimTeamId = s.VictimTeamId,
                                           KillerTeamId = s.KillerTeamId,
                                           s.IsGroggy
                                           
                                       }).OrderBy(o => o.TimeKilled);

            var killiPrinter = new List<KilliPrinter>();

            var teamCount = new List<int>();

            foreach (var item in result.Where(c => c.IsGroggy == false))
            {
                var playerLeftCount = 80 - killiPrinter.Count();

                var playerLeft = playerLeftCount == 1 ? "winner" : playerLeftCount.ToString() + " LEFT";

                var gameTimePlayerEliminated = DateTime.Parse(item.TimeKilled) - DateTime.Parse(CreateAt);

                var playerKillMessage = new PlayerKilledGraphics()
                {
                    TimeKilled = $"{gameTimePlayerEliminated}",
                    KillerName = $"{item.KillerName}",
                    FreeText1 = $"KILLED",
                    VictimName = $"{item.VictimName}",
                    FreeText2 = $"WITH",
                    DamagedCausedBy = $"{_readAssets.GetDamageCauserName(item.DamageCause)} ",
                    PlayerLeft = $"{playerLeft}"
                };


                TeamEliminated teamEliminatedMessage = null;

                teamCount.Add(item.VictimTeamId);

                var teamPlayerCount = playerKilled.Where(cn => cn.VictimTeamId == item.VictimTeamId && cn.IsGroggy == false).Count();

                if (teamCount.Where(cn => cn == item.VictimTeamId).Count() == teamPlayerCount )
                {
                    var teamEliminatedCount = killiPrinter.Where(cn => cn.TeamEliminated != null).Count() == 0 ? 1 : killiPrinter.Where(cn => cn.TeamEliminated != null).Count();

                    var teamLeftCount = playerKilled.GroupBy(g => g.KillerTeamId).Count() - teamEliminatedCount ;
                    
                    var teamLeft = teamLeftCount == 1 ? "winner" : teamLeftCount.ToString() + " LEFT";

                    var gameTimeTeamEliminated = DateTime.Parse(item.TimeKilled) - DateTime.Parse(CreateAt);


                    teamEliminatedMessage = new TeamEliminated()
                    {
                        TimeElimnated = $"{gameTimeTeamEliminated}",
                        FreeText1 = $"Team",
                        TeamId = $"{item.VictimTeamId}",
                        FreeText2 = $"HAS BEEN ELIMINATED",
                        TeamLeft = $"{teamLeft}"
                    };
                }

                var killMessage = new KilliPrinter() { PlayerKilled = playerKillMessage, TeamEliminated = teamEliminatedMessage };


                killiPrinter.Add(killMessage);
            }

            return killiPrinter;
        }
    }
}
