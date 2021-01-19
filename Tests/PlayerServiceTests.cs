using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebAPI.DataAccess;
using WebAPI.Logic;
using WebAPI.Models;

namespace Tests
{
    public class PlayerServiceTests
    {
        [Fact]
        public async Task GetPlayerByName_PlayerNameExists_ReturnPlayer()
        {
            string expectedName = "Test";

            var expectedPlayer = new Player()
            {
                Id = 1,
                Name = "Test"
            };
            
            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.FindPlayerByName(It.IsAny<string>())).ReturnsAsync(expectedPlayer);

            var playerService = new PlayerService(dataAccessMock.Object);
            var result = await playerService.GetPlayerByName(expectedName);
            
            Assert.Equal(expectedName, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task AddNewPlayer_PlayerDoesNotExist_ReturnsPlayer()
        {
            string expectedName = "Test";
            int expextedId = 1;
            
            var expectedPlayer = new Player()
            {
                Id = 1,
                Name = expectedName
            };
            
            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.SavePlayer(It.IsAny<string>())).ReturnsAsync(expextedId);

            var playerService = new PlayerService(dataAccessMock.Object);
            var result = await playerService.AddNewPlayer(expectedName);
            
            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(1, result.Id);
        }
        
        [Fact]
        public async Task AddPlayerScore_ValidDataSupplied_ReturnsPlayerScore()
        {
            var expectedPlayerScore = new PlayerScore()
            {
                PlayerId = 1,
                Scores = new List<GameScore>()
                {
                    new GameScore()
                    {
                        Score = 100,
                        SessionStart = DateTime.Today,
                        SessionEnd = DateTime.Now,
                    }
                }
            };
            
            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.SavePlayerScore(It.IsAny<PlayerScore>())).ReturnsAsync(true);

            var playerService = new PlayerService(dataAccessMock.Object);
            var result = await playerService.AddPlayerScore(expectedPlayerScore);
            
            Assert.NotNull(result);
            Assert.Equal(expectedPlayerScore.PlayerId, expectedPlayerScore.PlayerId);
            Assert.Equal(expectedPlayerScore.Scores.Count, expectedPlayerScore.Scores.Count);
        }
        
        [Fact]
        public async Task GetPlayerScores_PlayerExists_ReturnsPlayerScores()
        {
            string playerName = "Test";
            int expectedScoreCount = 1;
            var expectedScores = new List<GameScore>()
            {
                new GameScore()
                {
                    Score = 100,
                    SessionStart = DateTime.Today,
                    SessionEnd = DateTime.Now,
                }
            };
            
            var dataAccessMock = new Mock<IDataAccess>();

            dataAccessMock.Setup(x => x.GetPlayerScores(It.IsAny<string>())).ReturnsAsync(expectedScores);

            var playerService = new PlayerService(dataAccessMock.Object);
            var result = await playerService.GetPlayerScores(playerName);
            
            Assert.NotNull(result);
            Assert.Equal(expectedScoreCount, result.Count);
        }
    }
}