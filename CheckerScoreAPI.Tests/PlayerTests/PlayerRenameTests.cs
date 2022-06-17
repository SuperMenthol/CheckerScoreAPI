using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckerScoreAPI.Tests.PlayerTests
{
    public class PlayerRenameTests
    {
        PlayerController _playerController;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IDataContext>();

            var playerList = new List<Player>()
            {
                new Player() { PlayerId = 1, Login = "userone", CreationDate = new DateTime(2022,6,1,12,0,0) },
                new Player() { PlayerId = 2, Login = "usertwo", CreationDate = new DateTime(2022,6,1,14,43,0) },
                new Player() { PlayerId = 3, Login = "userthree", CreationDate = new DateTime(2022,6,13,12,0,0) }
            };

            var isAny = It.IsAny<FilterDefinition<Player>>();

            var playerMock = new Mock<IMongoCollection<Player>>();
            playerMock.Object.InsertMany(playerList);

            mockContext.Setup(x => x.Players).Returns(playerMock.Object);

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
        }

        [Test]
        public async Task RenamePlayer_NoId_ShouldBeFalse()
        {
            var playerModel = new PlayerModel()
            {
                PlayerName = "supername",
                CreatedAt = DateTime.Now
            };

            var action = await _playerController.RenamePlayer(playerModel);
            BaseResponse response = (BaseResponse)action.Value;

            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task RenamePlayer_NameExists_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, "usertwo", DateTime.Now);

            var action = await _playerController.RenamePlayer(playerModel);
            BaseResponse response = (BaseResponse)action.Value;

            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task RenamePlayer_NotValidName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, "notvבְname", DateTime.Now);

            var action = await _playerController.RenamePlayer(playerModel);
            BaseResponse response = (BaseResponse)action.Value;

            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task RenamePlayer_EmptyName_ShouldBeFalse()
        {
            var playerModel = new PlayerModel(1, string.Empty, DateTime.Now);

            var action = await _playerController.RenamePlayer(playerModel);
            BaseResponse response = (BaseResponse)action.Value;

            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task RenamePlayer_ValidName_ShouldBeTrue()
        {
            var playerModel = new PlayerModel(3, "newname", DateTime.Now);

            var action = await _playerController.RenamePlayer(playerModel);
            BaseResponse response = (BaseResponse)action.Value;

            Assert.IsFalse(response.Success);
        }
    }
}