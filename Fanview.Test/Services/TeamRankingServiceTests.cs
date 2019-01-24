using Fanview.API.Model;
using Fanview.API.Model.LiveModels;
using Fanview.API.Model.ViewModels;
using Fanview.API.Repository.Interface;
using Fanview.API.Services;
using Fanview.API.Services.Interface;
using Fanview.API.Utility;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fanview.Test.Services
{
    [TestFixture]
    public class TeamRankingServiceTests
    {
        private ITeamRankingService _sut;
        private IPlayerKillRepository _playerKillRepository;

        [SetUp]
        public void Setup()
        {
            _playerKillRepository = Substitute.For<IPlayerKillRepository>();
            _sut = new TeamRankingService(_playerKillRepository);
        }

        [Test]
        public async Task GetTeamRankings_WhenProvidingMockInputs_ShouldProduceExpectedOutput()
        {
            // arrange
            var mockKillListStr = EmbeddedResourcesUtility.ReadEmbeddedResource("Fanview.Test.Mocks.killList.json");
            var mockLiveMatchStatusStr = EmbeddedResourcesUtility.ReadEmbeddedResource("Fanview.Test.Mocks.liveMatchStatus.json");
            var mockResStr = EmbeddedResourcesUtility.ReadEmbeddedResource("Fanview.Test.Mocks.teamRankingResult.json");
            var mockKillList = JsonConvert.DeserializeObject<KillLeader>(mockKillListStr);
            var mockLiveMatchStatus = JsonConvert.DeserializeObject<IEnumerable<LiveMatchStatus>>(mockLiveMatchStatusStr);
            var mockRes = JsonConvert.DeserializeObject<IEnumerable<LiveTeamRanking>>(mockResStr);

            // act
            var res = await _sut.GetTeamRankings(mockKillList, mockLiveMatchStatus);

            // assert
            foreach (var team in res)
            {
                var mockTeam = mockRes.FirstOrDefault(t => t.TeamId == team.TeamId);
                Assert.AreEqual(mockTeam.RankPoints, team.RankPoints, team.TeamName + " - Rank points");
                Assert.AreEqual(mockTeam.KillPoints, team.KillPoints, team.TeamName + " - Kill points");
                Assert.AreEqual(mockTeam.TotalPoints, team.TotalPoints, team.TeamName + " - Total points");
                Assert.AreEqual(mockTeam.TeamRank, team.TeamRank, team.TeamName + " - Rank");
            }
        }
    }
}
