using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CheckerScoreAPI.Tests.PlayerTests
{
    public class PlayerAddTests
    {
        MatchController _matchController;
        PlayerController _playerController;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IDataContext>();

            mockContext.Setup(x => x.GetPlayerByName("samename")).Returns(new Model.Entity.Player() { });

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
            _matchController = new MatchController(new Mock<ILogger<MatchController>>().Object, mockContext.Object);
        }

        [Test]
        public void AddingPlayerLongerThan20Characters_ShouldBe_False()
        {
            string playerName = "Playernamelongerthantwentycharacters";

            var action = _playerController.CreatePlayer(playerName);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void AddingPlayerWithNonWordCharacters_ShouldBe_False()
        {
            string playerName = "testαuser";

            var action = _playerController.CreatePlayer(playerName);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void AddingAnotherPlayerWithSameName_ShouldBe_False()
        {
            string playerName = "samename";

            var action = _playerController.CreatePlayer(playerName);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void AddingNewPlayerWithEmptyName_ShouldBe_True()
        {
            string playerName = string.Empty;

            var action = _playerController.CreatePlayer(playerName);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void AddingNewConformingPlayer_ShouldBe_True()
        {
            string playerName = "newname1234";

            var action = _playerController.CreatePlayer(playerName);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsTrue((bool)data.Value.Success);
        }
    }
}