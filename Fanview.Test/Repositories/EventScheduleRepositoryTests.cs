using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Fanview.Test.Repositories
{
    [TestFixture]
    public class EventScheduleRepositoryTests
    {
        private IEventScheduleRepository _sut;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger<EventScheduleRepository>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _sut = new EventScheduleRepository(logger, memoryCache);
        }

        [Test]
        public void GetCompetitionSchedule_ShouldReturnSchedule()
        {
            // act
            var res = _sut.GetCompetitionSchedule();

            // assert
            Assert.IsNotNull(res);
        }

        [Test]
        public void GetDailySchedule_ShouldReturnDailySchedule()
        {
            // act
            var res = _sut.GetDailySchedule("1");

            // assert
            Assert.IsNotNull(res);
        }
    }
}
