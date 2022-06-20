using CheckerScoreAPI.Controllers;
using CheckerScoreAPI.Data.Abstracts;
using CheckerScoreAPI.Model;
using CheckerScoreAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckerScoreAPI.Tests.MatchTests
{
    public class MatchTests
    {
        private MatchController _matchController;

        private List<Player> _playerList = new List<Player>()
        {
            new Player() { PlayerId = 1, Login = "userone", CreationDate = new DateTime(2022,6,1,12,0,0) },
            new Player() { PlayerId = 2, Login = "usertwo", CreationDate = new DateTime(2022,6,1,14,43,0) },
            new Player() { PlayerId = 3, Login = "userthree", CreationDate = new DateTime(2022,6,13,12,0,0) }
        };
        private List<Result> _resultList = new List<Result>()
        {
            new Result()
            {
                PlayerOneId = 1,
                PlayerTwoId = 2,
                WinnerId = 1,
                PlayedAt = new DateTime(2022,6,13,12,1,1)
            },
            new Result()
            {
                PlayerOneId = 1,
                PlayerTwoId = 2,
                WinnerId = 2,
                PlayedAt = new DateTime(2022,6,13,15,3,0)
            },
            new Result()
            {
                PlayerOneId = 1,
                PlayerTwoId = 2,
                WinnerId = 0,
                PlayedAt = new DateTime(2022,6,13,17,22,23)
            },
            new Result()
            {
                PlayerOneId = 1,
                PlayerTwoId = 2,
                WinnerId = 1,
                PlayedAt = new DateTime(2022,6,13,20,21,40)
            },
            new Result()
            {
                PlayerOneId = 2,
                PlayerTwoId = 3,
                WinnerId = 3,
                PlayedAt = new DateTime(2022,6,13,21,11,14)
            }
        };

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IDataContext>();

            var playerMock = new Mock<IMongoCollection<Player>>();
            playerMock.Object.InsertMany(_playerList);

            var matchMock = new Mock<IMongoCollection<Result>>();
            matchMock.Object.InsertMany(_resultList);

            mockContext.Setup(x => x.Players).Returns(playerMock.Object);
            mockContext.Setup(x => x.Results).Returns(matchMock.Object);

            _matchController = new MatchController(new Mock<ILogger<MatchController>>().Object, mockContext.Object);
        }

        [Test]
        public async Task GetMatchResults_Player3_ShouldBeOne()
        {
            var playerId = 3;

            ObjectResult result = await _matchController.GetPlayerResults(playerId);
            List<MatchResult> results = (List<MatchResult>)result.Value;

            Assert.Equals(results.Count, 1);
        }
    }
}
