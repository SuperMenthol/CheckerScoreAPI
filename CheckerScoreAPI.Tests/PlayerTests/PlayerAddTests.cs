using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model.Entity;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CheckerScoreAPI.Tests.PlayerTests
{
    public class PlayerAddTests
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

            mockContext.Object.Players().InsertMany(playerList);

            _playerController = new PlayerController(new Mock<ILogger<PlayerController>>().Object, mockContext.Object);
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
            string playerName = "userone";

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