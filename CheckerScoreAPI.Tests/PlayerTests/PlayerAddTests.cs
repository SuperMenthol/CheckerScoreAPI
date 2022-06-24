using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckerScoreAPI.Tests.PlayerTests
{
    public class PlayerAddTests
    {
        PlayerController _playerController;
        private List<Player> _playerList = new List<Player>()
        {
            new Player() { PlayerId = 1, Login = "userone", CreationDate = new DateTime(2022,6,1,12,0,0) },
            new Player() { PlayerId = 2, Login = "usertwo", CreationDate = new DateTime(2022,6,1,14,43,0) },
            new Player() { PlayerId = 3, Login = "userthree", CreationDate = new DateTime(2022,6,13,12,0,0) }
        };

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IDataContext>();

            var playerMock = new Mock<IMongoCollection<Player>>();
            playerMock.Object.InsertMany(_playerList);

            mockContext.Setup(x => x.Players).Returns(playerMock.Object);

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
        }

        [Test]
        public async Task AddingPlayerLongerThan20Characters_ShouldBe_False()
        {
            string playerName = "Playernamelongerthantwentycharacters";

            var action = await _playerController.CreatePlayer(playerName);

            Assert.IsFalse(action.Success);
        }

        [Test]
        public async Task AddingPlayerWithNonWordCharacters_ShouldBe_False()
        {
            string playerName = "testαuser";

            var action = await _playerController.CreatePlayer(playerName);

            Assert.IsFalse(action.Success);
        }

        [Test]
        public async Task AddingAnotherPlayerWithSameName_ShouldBe_False()
        {
            string playerName = "userone";

            var action = await _playerController.CreatePlayer(playerName);

            Assert.IsFalse(action.Success);
        }

        [Test]
        public async Task AddingNewPlayerWithEmptyName_ShouldBe_False()
        {
            string playerName = string.Empty;

            var action = await _playerController.CreatePlayer(playerName);

            Assert.IsFalse(action.Success);
        }

        [Test]
        public async Task AddingNewConformingPlayer_ShouldBe_True()
        {
            string playerName = "newname1234";

            var action = await _playerController.CreatePlayer(playerName);

            Assert.IsFalse(action.Success);
        }
    }
}