using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CheckerScoreAPI.Tests.PlayerTests
{
    public class PlayerRenameTests
    {
        MatchController _matchController;
        PlayerController _playerController;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IDataContext>();

            mockContext.Setup(x => x.GetPlayerByName("samename")).Returns(new Player() { });
            mockContext.Setup(x => x.GetPlayerByName("noname")).Returns(new Player() { });
            mockContext.Setup(x => x.GetPlayerByID(3)).Returns(new Player() { });

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
            _matchController = new MatchController(new Mock<ILogger<MatchController>>().Object, mockContext.Object);
        }

        [Test]
        public void RenamePlayer_NoId_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerName = "supername"
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_NameExists_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerId = 1,
                PlayerName = "samename"
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_NotValidName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerId = 1,
                PlayerName = "notvבְname"
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_EmptyName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerId = 1,
                PlayerName = string.Empty
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_ValidName_ShouldBeTrue()
        {
            var playerModel = new PlayerModel()
            {
                PlayerId = 3,
                PlayerName = "newname"
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsTrue((bool)data.Value.Success);
        }
    }
}