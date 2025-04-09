using System;
using System.Collections.Generic;
using Duo.Constants;
using Duo.Interfaces;
using Duo.Models;
using Duo.Services;
using Moq;
using Xunit;

namespace TestsDuo2.Services
{
    public class LeaderboardServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IFriendsRepository> _mockFriendsRepository;
        private readonly LeaderboardService _leaderboardService;
        
        public LeaderboardServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockFriendsRepository = new Mock<IFriendsRepository>();
            _leaderboardService = new LeaderboardService(_mockUserRepository.Object, _mockFriendsRepository.Object);
        }
        
        [Fact]
        public void Constructor_WithNullUserRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LeaderboardService(null, _mockFriendsRepository.Object));
        }
        
        [Fact]
        public void Constructor_WithNullFriendsRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LeaderboardService(_mockUserRepository.Object, null));
        }
        
        [Fact]
        public void GetGlobalLeaderboard_WithCompletedQuizzesCriteria_CallsCorrectRepositoryMethod()
        {
            // Arrange
            var expectedLeaderboard = new List<LeaderboardEntry>
            {
                new LeaderboardEntry { UserId = 1, CompletedQuizzes = 10 },
                new LeaderboardEntry { UserId = 2, CompletedQuizzes = 5 }
            };
            _mockUserRepository.Setup(r => r.GetTopUsersByCompletedQuizzes()).Returns(expectedLeaderboard);
            
            // Act
            var result = _leaderboardService.GetGlobalLeaderboard(LeaderboardConstants.CompletedQuizzesCriteria);
            
            // Assert
            Assert.Same(expectedLeaderboard, result);
            _mockUserRepository.Verify(r => r.GetTopUsersByCompletedQuizzes(), Times.Once);
        }
        
        [Fact]
        public void GetGlobalLeaderboard_WithAccuracyCriteria_CallsCorrectRepositoryMethod()
        {
            // Arrange
            var expectedLeaderboard = new List<LeaderboardEntry>
            {
                new LeaderboardEntry { UserId = 1, Accuracy = 95.5M },
                new LeaderboardEntry { UserId = 2, Accuracy = 90.0M }
            };
            _mockUserRepository.Setup(r => r.GetTopUsersByAccuracy()).Returns(expectedLeaderboard);
            
            // Act
            var result = _leaderboardService.GetGlobalLeaderboard(LeaderboardConstants.AccuracyCriteria);
            
            // Assert
            Assert.Same(expectedLeaderboard, result);
            _mockUserRepository.Verify(r => r.GetTopUsersByAccuracy(), Times.Once);
        }
        
      
        
        [Fact]
        public void GetFriendsLeaderboard_WithCompletedQuizzesCriteria_CallsCorrectRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedLeaderboard = new List<LeaderboardEntry>
            {
                new LeaderboardEntry { UserId = 2, CompletedQuizzes = 10 },
                new LeaderboardEntry { UserId = 3, CompletedQuizzes = 5 }
            };
            _mockFriendsRepository.Setup(r => r.GetTopFriendsByCompletedQuizzes(userId)).Returns(expectedLeaderboard);
            
            // Act
            var result = _leaderboardService.GetFriendsLeaderboard(userId, LeaderboardConstants.CompletedQuizzesCriteria);
            
            // Assert
            Assert.Same(expectedLeaderboard, result);
            _mockFriendsRepository.Verify(r => r.GetTopFriendsByCompletedQuizzes(userId), Times.Once);
        }
        
        [Fact]
        public void GetFriendsLeaderboard_WithAccuracyCriteria_CallsCorrectRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedLeaderboard = new List<LeaderboardEntry>
            {
                new LeaderboardEntry { UserId = 2, Accuracy = 95.5M },
                new LeaderboardEntry { UserId = 3, Accuracy = 90.0M }
            };
            _mockFriendsRepository.Setup(r => r.GetTopFriendsByAccuracy(userId)).Returns(expectedLeaderboard);
            
            // Act
            var result = _leaderboardService.GetFriendsLeaderboard(userId, LeaderboardConstants.AccuracyCriteria);
            
            // Assert
            Assert.Same(expectedLeaderboard, result);
            _mockFriendsRepository.Verify(r => r.GetTopFriendsByAccuracy(userId), Times.Once);
        }
        
      
        
        [Fact]
        public void UpdateUserScore_ExecutesSuccessfully()
        {
            // Arrange
            int userId = 1;
            int points = 10;
            
            // Act - Should not throw
            _leaderboardService.UpdateUserScore(userId, points);
            
            // Assert - Currently this is a TODO method, so nothing to verify
        }
        
        [Fact]
        public void CalculateRankChange_ExecutesSuccessfully()
        {
            // Arrange
            int userId = 1;
            string timeFrame = "weekly";
            
            // Act - Should not throw
            _leaderboardService.CalculateRankChange(userId, timeFrame);
            
            // Assert - Currently this is a TODO method, so nothing to verify
        }
    }
} 