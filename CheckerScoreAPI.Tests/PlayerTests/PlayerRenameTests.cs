using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;

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

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
            _matchController = new MatchController(new Mock<ILogger<MatchController>>().Object, mockContext.Object);
        }

        [Test]
        public void RenamePlayer_NoId_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerName = "supername",
                CreatedAt = DateTime.Now
            };

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_NameExists_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, "usertwo", DateTime.Now);

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_NotValidName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, "notvבְname", DateTime.Now);

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_EmptyName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, string.Empty, DateTime.Now);

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsFalse((bool)data.Value.Success);
        }

        [Test]
        public void RenamePlayer_ValidName_ShouldBeTrue()
        {
            var playerModel = new PlayerModel(3, "newname", DateTime.Now);

            var action = _playerController.RenamePlayer(playerModel);
            dynamic data = JObject.Parse(JsonConvert.SerializeObject(action));

            Assert.IsTrue((bool)data.Value.Success);
        }
    }
}