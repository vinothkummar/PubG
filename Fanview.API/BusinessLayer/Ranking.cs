using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Model;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanview.API.BusinessLayer
{
    public class Ranking : IRanking
    {
        private ILogger<Ranking> _logger;
        private IMatchSummaryRepository _matchSummaryRepository;
        private IPlayerKillRepository _playerKillRepository;
        private IGenericRepository<RankPoints> _genericRankPointsRepository;
        private IGenericRepository<MatchRanking> _genericMatchRankingRepository;
        private IGenericRepository<RankPointAdjustments> _rankPointAdjustments;
        private IGenericRepository<TeamRanking> _genericTeamRankingRepository;
        private IGenericRepository<Team> _genericTeamRepository;
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _teamPlayerRespository;
        private IGenericRepository<Event> _tournament;

        public Ranking(ILogger<Ranking> logger, IMatchSummaryRepository matchSummaryRepository, 
                       IPlayerKillRepository playerKillRepository, 
                       IGenericRepository<RankPoints> genericRankPointsRepository,
                       IGenericRepository<MatchRanking> genericMatchRankingRepository,
                       IGenericRepository<TeamRanking> genericTeamRankingRepository,
                       IGenericRepository<Team> genericTeamRepository,
                       IGenericRepository<RankPointAdjustments> rankPointAdjustments,
                       ITeamRepository teamRepository,
                       ITeamPlayerRepository teamPlayerRepository, 
                       IGenericRepository<Event> tournament)
        {
            _logger = logger;
            _matchSummaryRepository = matchSummaryRepository;
            _playerKillRepository = playerKillRepository;
            _genericRankPointsRepository = genericRankPointsRepository;
            _genericMatchRankingRepository = genericMatchRankingRepository;
            _genericTeamRankingRepository = genericTeamRankingRepository;
            _genericTeamRepository = genericTeamRepository;
            _rankPointAdjustments = rankPointAdjustments;
            _teamRepository = teamRepository;
            _teamPlayerRespository = teamPlayerRepository;
            _tournament = tournament;
        }

        public async Task<IEnumerable<MatchRanking>> CalculateMatchRanking(string matchId)
        {           

            var matchPlayerStats = _matchSummaryRepository.GetPlayerMatchStats(matchId).Result;

            var teams = _teamRepository.GetTeam().Result.Select(s => new { TeamId = s.Id, TeamName = s.Name });
          

            var rankScorePoints =  _genericRankPointsRepository.GetAll("RankPoints").Result;

            var playerKillPointsWithTeam = matchPlayerStats.Select(m => new { m.MatchId, TeamId = m.TeamId, ShortTeamId = m.ShortTeamId, PlayerAccountId = m.stats.PlayerId, KillPoints = m.stats.Kills * 1 })
                                                           .GroupBy(g => new { g.TeamId, g.ShortTeamId,  g.MatchId })
                                                           .Select(s => new
                                                            { 
                                                                MatchId= s.Key.MatchId,
                                                                TeamId = s.Key.TeamId,
                                                                ShortTeamId = s.Key.ShortTeamId,
                                                                Killpoints = s.Select(g => g.KillPoints),
                                                                TeamKillTotalPoints = s.Sum(t => t.KillPoints),
                                                                PlayerAccountId = s.Select(g => g.PlayerAccountId)
                                                            });


            
            var teamMatchRankingScoreCalculation = playerKillPointsWithTeam.Join(teams, pkp => pkp.TeamId, t => t.TeamId, (pk, t) => new { pk, t })
                                                                 .Join(matchPlayerStats, tpkp => tpkp.pk.ShortTeamId, tep => tep.ShortTeamId, (tpkp, tep) => new { tpkp, tep })
                                                                 .Join(rankScorePoints, tpktep => tpktep.tep.Rank, rsp => rsp.RankPosition, (tpktep, rsp) => new { tpktep, rsp })
                                                                 .OrderByDescending(o => o.tpktep.tpkp.pk.TeamKillTotalPoints + o.rsp.ScoringPoints)
                                                                 .Select(s => new MatchRanking()
                                                                 {

                                                                     MatchId = s.tpktep.tpkp.pk.MatchId,
                                                                     TeamId = s.tpktep.tpkp.pk.TeamId,
                                                                     TeamName = s.tpktep.tpkp.t.TeamName,
                                                                     KillPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints,
                                                                     RankPoints = s.rsp.ScoringPoints,
                                                                     TotalPoints = s.tpktep.tpkp.pk.TeamKillTotalPoints + s.rsp.ScoringPoints,
                                                                     ShortTeamID = s.tpktep.tep.ShortTeamId
                                                                 }).GroupBy(g => g.ShortTeamID);


            var teamMatchRankingScore = teamMatchRankingScoreCalculation.Select(s => new MatchRanking()
            {
                MatchId = s.Select(a => a.MatchId).ElementAtOrDefault(0),
                TeamId = s.Select(a => a.TeamId).ElementAtOrDefault(0),
                TeamName = s.Select(a => a.TeamName).ElementAtOrDefault(0),
                KillPoints = s.Select(a => a.KillPoints).ElementAtOrDefault(0),
                RankPoints = s.Select(a => a.RankPoints).ElementAtOrDefault(0),
                TotalPoints = s.Select(a => a.TotalPoints).ElementAtOrDefault(0),
                ShortTeamID = s.Select(a => a.ShortTeamID).ElementAtOrDefault(0)
            });
                       
            return await Task.FromResult(teamMatchRankingScore);
        }

        public async Task<IEnumerable<TeamRanking>> CalculateTeamStats(string matchId, IEnumerable<MatchRanking> matchRankings)
        {
            var matchPlayerStats = _matchSummaryRepository.GetPlayerMatchStats(matchId).Result;

            var teams = _teamRepository.GetTeam().Result.Select(s => new { TeamId = s.Id, TeamName = s.Name, TeamShortID = s.TeamId });

            var teamPoints = matchPlayerStats.Join(teams, mp => mp.TeamId, t => t.TeamId, (mp,t) => new { mp, t })                                          
                                           .Select(s => new {MatchId = s.mp.MatchId, TeamId = s.t.TeamId, TeamShortId = s.t.TeamShortID, TeamName = s.t.TeamName, Kills = s.mp.stats.Kills, Damage = s.mp.stats.DamageDealt })
                                           .GroupBy(g => new { g.TeamId, g.MatchId, g.TeamShortId, g.TeamName  })
                                                           .Select(s => new
                                                           {
                                                               MatchId = s.Key.MatchId,
                                                               TeamId = s.Key.TeamId,
                                                               TeamShortId = s.Key.TeamShortId,
                                                               TeamName = s.Key.TeamName,
                                                               Kills = s.Sum(g => g.Kills),
                                                               Damage = s.Sum(t => t.Damage),
                                                               
                                                           });
            var teamStandings = matchRankings.Join(teamPoints, mr => mr.TeamId, tp => tp.TeamId, (mr, tp) => new { mr, tp }).Select(s => new TeamRanking()
            {
                TeamId = s.tp.TeamShortId.ToString(),
                Kill = s.tp.Kills,
                Damage = s.tp.Damage,
                MatchId = s.tp.MatchId,
                TeamName = s.tp.TeamName,
                TotalPoints = s.mr.TotalPoints
                
            });

            return await Task.FromResult(teamStandings);
        }
        public async Task<IEnumerable<RankingResults>> GetMatchRankings(int matchId)
        {
            var tournaments = _tournament.GetMongoDbCollection("TournamentMatchId");

            var tournamentMatchId = tournaments.FindAsync(Builders<Event>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result.Id;

            var matchRankingCollection = _genericMatchRankingRepository.GetMongoDbCollection("MatchRanking");

            var matchRankingScore = matchRankingCollection.FindAsync(Builders<MatchRanking>.Filter.Where(cn => cn.MatchId == tournamentMatchId)).Result.ToListAsync();
            
            IEnumerable<RankingResults> rankingResult = RankingOrder(matchRankingScore.Result);

            return await Task.FromResult(rankingResult);
        }

        public async Task<IEnumerable<TournamentRanking>> GetTournamentRankings()
        {

            var matchRankingCollection = _genericMatchRankingRepository.GetAll("MatchRanking");

            List<TournamentRanking> matchStandings = OrderTournamentRanking(matchRankingCollection);


            var rankingResult = matchStandings.Select(s => new TournamentRanking()
            {
                TeamRank = s.TeamRank,
                TeamId = s.ShortTeamID,
                TeamName = s.TeamName,
                KillPoints = s.KillPoints,
                RankPoints = s.RankPoints,
                TotalPoints = s.TotalPoints,
                BestKillPoints = s.BestKillPoints,
                BestTotalPoints = s.BestTotalPoints,
                LastKillPoints = s.LastKillPoints,
                LastRankPoints = s.LastRankPoints,
                AdjustedPoints = s.AdjustedPoints
            });

            return await Task.FromResult(rankingResult);
        }

        private List<TournamentRanking> OrderTournamentRanking(Task<IEnumerable<MatchRanking>> matchRankingCollection)
        {
            var lastMatchId = matchRankingCollection.Result.Select(s => s.MatchId).OrderByDescending(o => o).Skip(1).FirstOrDefault();

            var rankPointAdjustments = _rankPointAdjustments.GetAll("RankPointAdjustments").Result.ToList();

            var i = 1;
            var tournamentRankingStandings = matchRankingCollection.Result
                                        .GroupBy(g => new { g.TeamName, g.ShortTeamID })
                                        .Select(s => new TournamentRanking()
                                        {
                                            PubGOpenApiTeamId = s.FirstOrDefault().PubGOpenApiTeamId,
                                            TeamName = s.Key.TeamName,
                                            ShortTeamID = s.Key.ShortTeamID,
                                            KillPoints = s.Sum(a => a.KillPoints),
                                            RankPoints = s.Sum(a => a.RankPoints),
                                            TotalPoints = s.Sum(a => a.TotalPoints)
                                        }).OrderByDescending(o => o.TotalPoints).Select(k => new TournamentRanking()
                                        {
                                            PubGOpenApiTeamId = k.PubGOpenApiTeamId,
                                            TeamName = k.TeamName,
                                            ShortTeamID = k.ShortTeamID,
                                            KillPoints = k.KillPoints,
                                            RankPoints = k.RankPoints,
                                            TotalPoints = GetAdjustedTotalPoints(k.TotalPoints, k.ShortTeamID, rankPointAdjustments),
                                            BestKillPoints = GetTheMaxTPoints(matchRankingCollection.Result.Where(cn => cn.TeamName == k.TeamName).Select(s1 => s1.KillPoints).ToArray()),
                                            BestTotalPoints = GetTheMaxTPoints(matchRankingCollection.Result.Where(cn => cn.TeamName == k.TeamName).Select(s1 => s1.TotalPoints).ToArray()),
                                            LastKillPoints = GetLastKillPoints(matchRankingCollection.Result, k.TeamName, lastMatchId),
                                            LastRankPoints = GetLastRankPoints(matchRankingCollection.Result, k.TeamName, lastMatchId),
                                            AdjustedPoints = GetAdjustedPoints(k.ShortTeamID, rankPointAdjustments)
                                        });



            var matchStandings = new List<TournamentRanking>();
            foreach (var item in tournamentRankingStandings.OrderByDescending(o => o.TotalPoints)
                                .ThenByDescending(t => t.KillPoints).ThenByDescending(t1 => t1.BestTotalPoints)
                                .ThenByDescending(t2 => t2.BestKillPoints).ThenByDescending(t3 => t3.LastKillPoints)
                                .ThenByDescending(t4 => t4.LastRankPoints))
            {

                var matchRanking = new TournamentRanking();
                matchRanking.MatchId = item.MatchId;
                matchRanking.TeamId = item.TeamId;
                matchRanking.TeamName = item.TeamName;
                matchRanking.KillPoints = item.KillPoints;
                matchRanking.RankPoints = item.RankPoints;
                matchRanking.TotalPoints = item.TotalPoints;

                var totalPointsEqual = matchStandings.Where(cn => cn.TotalPoints == item.TotalPoints);

                var killPointsEqual = totalPointsEqual.Where(cn => cn.KillPoints == item.KillPoints);

                var bestTotalPointsEqual = killPointsEqual.Where(cn => cn.BestTotalPoints == item.BestTotalPoints);

                var bestKillPointsEqual = bestTotalPointsEqual.Where(cn => cn.BestKillPoints == item.BestKillPoints);

                var lastKillPointsEqual = bestKillPointsEqual.Where(cn => cn.LastKillPoints == item.LastKillPoints);

                var lastRankPointsEqual = lastKillPointsEqual.Where(cn => cn.LastRankPoints == item.LastRankPoints);

                if (totalPointsEqual.Count() > 0)
                {
                    i = (matchStandings.Count + 1) - totalPointsEqual.Count();
                    matchRanking.TeamRank = $"{i}";

                    if (killPointsEqual.Count() > 0)
                    {
                        i = (matchStandings.Count + 1) - killPointsEqual.Count();
                        matchRanking.TeamRank = $"{i}";

                        if (bestTotalPointsEqual.Count() > 0)
                        {

                            i = (matchStandings.Count + 1) - bestTotalPointsEqual.Count();
                            matchRanking.TeamRank = $"{i}";

                            if (bestKillPointsEqual.Count() > 0)
                            {
                                i = (matchStandings.Count + 1) - bestKillPointsEqual.Count();
                                matchRanking.TeamRank = $"{i}";

                                if(lastKillPointsEqual.Count() > 0)
                                {
                                    i = (matchStandings.Count + 1) - lastKillPointsEqual.Count();
                                    matchRanking.TeamRank = $"{i}";

                                    if(lastRankPointsEqual.Count() > 0)
                                    {
                                        i = (matchStandings.Count + 1) - lastRankPointsEqual.Count();
                                        matchRanking.TeamRank = $"{i}";
                                    }
                                    else
                                    {
                                        matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                                    }
                                }
                                else
                                {
                                    matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                                }

                            }
                            else
                            {
                                matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                            }
                        }
                        else
                        {
                            matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                        }
                    }
                    else
                    {
                        matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                    }
                }
                else
                {
                    matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                }


                matchRanking.PubGOpenApiTeamId = item.PubGOpenApiTeamId;
                matchRanking.ShortTeamID = item.ShortTeamID;
                matchRanking.BestKillPoints = item.BestKillPoints;
                matchRanking.BestTotalPoints = item.BestTotalPoints;
                matchRanking.LastKillPoints = item.LastKillPoints;
                matchRanking.LastRankPoints = item.LastRankPoints;
                matchRanking.AdjustedPoints = item.AdjustedPoints;

                matchStandings.Add(matchRanking);
            }

            return matchStandings;
        }

        public async Task<IEnumerable<RankingResults>> GetTournamentRankingByDay(int day)
        {
            const int matchesPerDay = 4;

            // Get daily rankings
            var dailyRankings = new List<IEnumerable<RankingResults>>();
            for (var i = 1; i <= matchesPerDay; ++i)
            {
                try
                {
                    var matchRanking = await GetMatchRankings(i + (matchesPerDay * (day - 1)));
                    dailyRankings.Add(matchRanking);
                }
                catch (Exception)
                {
                    // Ignore
                }
            }

            // Create a dictionary from team name to ranking result.
            // The ranking result is calculated by combining the stats of all matches in
            // that day.
            var teamRankingDict = new Dictionary<string, RankingResults>();
            foreach (var dailyRanking in dailyRankings)
            foreach (var matchRanking in dailyRanking)
            {
                if (!teamRankingDict.ContainsKey(matchRanking.TeamName))
                {
                    teamRankingDict.Add(matchRanking.TeamName, new RankingResults
                    {
                        TeamId = matchRanking.TeamId,
                        TeamName = matchRanking.TeamName
                    });
                }
                teamRankingDict[matchRanking.TeamName].KillPoints += matchRanking.KillPoints;
                teamRankingDict[matchRanking.TeamName].RankPoints += matchRanking.RankPoints;
                teamRankingDict[matchRanking.TeamName].TotalPoints += matchRanking.TotalPoints;
            }

         
            var matchRankingScore = teamRankingDict.Values.Select(s => new MatchRanking() {
                ShortTeamID = s.TeamId,
                TeamName = s.TeamName,
                KillPoints = s.KillPoints,
                RankPoints = s.RankPoints,
                TotalPoints = s.TotalPoints}).ToList();

            // Create a list holding the combined rankings for that day and sort it by total points
            IEnumerable<RankingResults> rankingResult = RankingOrder(matchRankingScore);


            return rankingResult;
        }

        public async Task<IEnumerable<MatchRanking>> PollAndGetMatchRanking(string matchId)
        {      
                await _matchSummaryRepository.PollMatchRoundRankingData(matchId);

            await Task.Delay(3000);

            Task<IEnumerable<MatchRanking>> matchRankings = Task<IEnumerable<MatchRanking>>.Factory.StartNew(() =>
            {
                var teamsScroingPoints = CalculateMatchRanking(matchId).Result.ToList();

                var TeamStats = CalculateTeamStats(matchId, teamsScroingPoints).Result;

                var matchRankingCollection = _genericMatchRankingRepository.GetMongoDbCollection("MatchRanking");

                var matchRankingScore = matchRankingCollection.FindAsync(Builders<MatchRanking>.Filter.Where(cn => cn.MatchId == matchId)).Result.FirstOrDefaultAsync().Result;

                if (matchRankingScore == null)
                {
                    _genericMatchRankingRepository.Insert(teamsScroingPoints, "MatchRanking");

                    _genericTeamRankingRepository.Insert(TeamStats, "TeamRanking");
                }

                return teamsScroingPoints;
            });

            return await await Task.FromResult(matchRankings);           
        }       
        private int GetTeamFinishingPositions(int i)
        {

            var position = 20 - i;
            i++;
            return position;
        }

        private int GetTheMaxTPoints(int[] TPoints)
        {
            int maxTPoints = TPoints.Max();

            return maxTPoints;
        }

        private int GetLastKillPoints(IEnumerable<MatchRanking> matchRankings, string teamName, string lastMatchId)
        {
            var lastKillPoint = matchRankings.Where(cn => cn.MatchId == lastMatchId && cn.TeamName == teamName).Select(s => s.KillPoints).FirstOrDefault();

            return lastKillPoint;
        }

        private int GetLastRankPoints(IEnumerable<MatchRanking> matchRankings, string teamName, string lastMatchId)
        {
            var lastRankPoint = matchRankings.Where(cn => cn.MatchId == lastMatchId && cn.TeamName == teamName).Select(s => s.RankPoints).FirstOrDefault();

            return lastRankPoint;
        }

        private int GetAdjustedTotalPoints(int tpoints, int teamId, IEnumerable<RankPointAdjustments> rankPointAdjustments)
        {
            var rankAdjustments = rankPointAdjustments.FirstOrDefault(cn => cn.TeamId == teamId);

            if (rankAdjustments != null)
            {

                var pointAdjustments = rankAdjustments.RankPointsAdjustments;

                if (pointAdjustments.Substring(0, 1) == "+")
                {
                    return tpoints + Convert.ToInt16(pointAdjustments.Substring(1, pointAdjustments.Length - 1));
                }
                else
                {
                    return tpoints - Convert.ToInt16(pointAdjustments.Substring(1, pointAdjustments.Length - 1));
                }
            }
            return tpoints;
        }

        private string GetAdjustedPoints(int teamId, IEnumerable<RankPointAdjustments> rankPointAdjustments)
        {
            var rankAdjustments = rankPointAdjustments.FirstOrDefault(cn => cn.TeamId == teamId);

            if (rankAdjustments != null)
            {
                return rankAdjustments.RankPointsAdjustments;               
            }
            else
            {
                return string.Empty;
            }
            
        }

        private IEnumerable<RankingResults> RankingOrder(List<MatchRanking> matchRankingScore)
        {
            var matchStandings = new List<MatchRanking>();

            var i = 1;

            foreach (var item in matchRankingScore.OrderByDescending(o => o.TotalPoints).ThenByDescending(t => t.KillPoints))
            {
                var matchRanking = new MatchRanking();
                matchRanking.MatchId = item.MatchId;
                matchRanking.TeamId = item.TeamId;
                matchRanking.TeamName = item.TeamName;
                matchRanking.KillPoints = item.KillPoints;
                matchRanking.RankPoints = item.RankPoints;
                matchRanking.TotalPoints = item.TotalPoints;
                matchRanking.ShortTeamID = item.ShortTeamID;

                var totalPointsEqual = matchStandings.Where(cn => cn.TotalPoints == item.TotalPoints);

                var killPointsEqual = totalPointsEqual.Where(cn => cn.KillPoints == item.KillPoints);



                if (totalPointsEqual.Count() > 0 && killPointsEqual.Count() > 0)
                {
                    i = (matchStandings.Count + 1) - killPointsEqual.Count();
                    matchRanking.TeamRank = $"{i}";
                }
                else
                {
                    matchRanking.TeamRank = $"{matchStandings.Count + 1}";
                }

                matchRanking.PubGOpenApiTeamId = item.PubGOpenApiTeamId;

                matchStandings.Add(matchRanking);
            }


            var rankingResult = matchStandings.Select(s => new RankingResults()
            {
                TeamRank = s.TeamRank,
                TeamId = s.ShortTeamID,
                TeamName = s.TeamName,
                KillPoints = s.KillPoints,
                RankPoints = s.RankPoints,
                TotalPoints = s.TotalPoints,
            });
            return rankingResult;
        }
    }
}