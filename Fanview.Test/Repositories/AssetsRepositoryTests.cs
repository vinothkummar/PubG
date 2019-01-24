using Fanview.API.Repository;
using Fanview.API.Repository.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Fanview.Test.Repositories
{
    [TestFixture]
    class AssetsRepositoryTests
    {
        private IAssetsRepository _sut;

        [SetUp]
        public void Setup()
        {
            var logger = Substitute.For<ILogger<AssetsRepository>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _sut = new AssetsRepository(logger, memoryCache);
        }

        [TestCase("AquaRail_A_01_C", "Aquarail")]
        [TestCase("Dacia_A_02_v2_C", "Dacia")]
        public void GetDamageCauserName_WhenProvidingKey_ShouldReturnExpectedValue(string key, string expectedValue)
        {
            // act
            var res = _sut.GetDamageCauserName(key);

            // assert
            Assert.IsNotNull(res);
            Assert.AreEqual(expectedValue, res);
        }

        [Test]
        public void GetDamageCauserName_WhenProvidingInvalidKey_ShouldReturnEmptyString()
        {
            // act
            var res = _sut.GetDamageCauserName("nonexistingkey");

            // assert
            Assert.IsNotNull(res);
            Assert.AreEqual(string.Empty, res);
        }
    }
}
