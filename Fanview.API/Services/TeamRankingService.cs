using Fanview.API.Constants;
using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.Services
{
    public class TeamRankingService : ITeamRankingService
    {
        private readonly IPlayerKillRepository _playerKillRepository;

        public TeamRankingService(IPlayerKillRepository playerKillRepository)
        {
            _playerKillRepository = playerKillRepository;
        }

        public Task<IEnumerable<LiveTeamRanking>> GetTeamRankings(KillLeader killLeader, 
            IEnumerable<LiveMatchStatus> liveStatus)
        {
            var ranking = new List<LiveTeamRanking>();

            // Set kill, rank and total points
            var killPoints = GetTeamKillPoints(killLeader);
            var rankPoints = GetTeamRankPoints(liveStatus);
            foreach (var team in liveStatus)
            {
                var rp = rankPoints[team.TeamId];
                var kp = 0;
                if (killPoints.ContainsKey(team.TeamId))
                {
                    kp = killPoints[team.TeamId];
                }

                ranking.Add(new LiveTeamRanking
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName,
                    RankPoints = rp,
                    KillPoints = kp,
                    TotalPoints = rp + kp
                });
            }

            // Sort by total points and set the rank accordingly
            ranking.Sort((a, b) => b.TotalPoints.CompareTo(a.TotalPoints));
            var i = 1;
            foreach (var team in ranking)
            {
                team.TeamRank = i++;
            }

            return Task.FromResult(ranking.AsEnumerable());
        }

        private IDictionary<int, int> GetTeamKillPoints(KillLeader killLeader)
        {
            var result = new Dictionary<int, int>();
            foreach (var kill in killLeader.killList)
            {
                var points = MatchConstants.PointsPerKill * kill.kills;
                if (result.ContainsKey(kill.teamId))
                {
                    result[kill.teamId] += points;
                }
                else
                {
                    result.Add(kill.teamId, points);
                }
            }
            return result;
        }

        private IDictionary<int, int> GetTeamRankPoints(IEnumerable<LiveMatchStatus> liveStatus)
        {
            var result = new Dictionary<int, int>();

            // These are the teams that were eliminated, ordered by the first ones to go out
            var eliminatedTeams = liveStatus
                .Where(t => t.IsEliminated)
                .OrderBy(t => t.EliminatedAt);

            // Teams that are still playing
            var liveTeams = liveStatus.Where(t => !t.IsEliminated);

            // Placement points for the teams that are still playing
            var placementForLiveTeams = MatchConstants.PlacementPointsTable[eliminatedTeams.Count()];

            // Go over all the eliminated teams and give them the score by indexing the points table
            // which is ordered from low to high
            int i = 0;
            foreach (var team in eliminatedTeams)
            {
                result.Add(team.TeamId, MatchConstants.PlacementPointsTable[i]);
                ++i;
            }

            // Now the placement points for the playing teams is the one next to the last eliminated
            // team (indexed by i)
            foreach (var team in liveTeams)
            {
                result.Add(team.TeamId, MatchConstants.PlacementPointsTable[i]);
            }

            return result;
        }
    }
}
