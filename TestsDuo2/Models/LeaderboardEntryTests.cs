using System;
using Xunit;
using DuoModels;

namespace TestsDuo2.Models
{
    public class LeaderboardEntryTests
    {
        [Fact]
        public void LeaderboardEntry_DefaultProperties_AreSetCorrectly()
        {
            // Arrange & Act
            var entry = new LeaderboardEntry();

            // Assert
            Assert.Equal(0, entry.UserId);
            Assert.Equal(0, entry.Rank);
            Assert.Null(entry.ProfilePicture);
            Assert.Null(entry.Username);
            Assert.Equal(0m, entry.Accuracy);
            Assert.Equal(0, entry.CompletedQuizzes);
        }

        [Fact]
        public void LeaderboardEntry_SetProperties_ReturnCorrectValues()
        {
            // Arrange
            var entry = new LeaderboardEntry
            {
                UserId = 42,
                Rank = 3,
                ProfilePicture = "avatar.jpg",
                Username = "JohnDoe",
                Accuracy = 95.5m,
                CompletedQuizzes = 25
            };

            // Act & Assert
            Assert.Equal(42, entry.UserId);
            Assert.Equal(3, entry.Rank);
            Assert.Equal("avatar.jpg", entry.ProfilePicture);
            Assert.Equal("JohnDoe", entry.Username);
            Assert.Equal(95.5m, entry.Accuracy);
            Assert.Equal(25, entry.CompletedQuizzes);
        }

        [Theory]
        [InlineData(1, 1, "path1.jpg", "User1", 90.5, 10)]
        [InlineData(2, 2, "path2.jpg", "User2", 85.3, 8)]
        [InlineData(3, 3, "path3.jpg", "User3", 95.7, 15)]
        [InlineData(4, 4, "path4.jpg", "User4", 75.0, 5)]
        public void LeaderboardEntry_SetVariousValues_ReturnCorrectValues(
            int userId, int rank, string profilePic, string username, 
            decimal accuracy, int completedQuizzes)
        {
            // Arrange
            var entry = new LeaderboardEntry
            {
                UserId = userId,
                Rank = rank,
                ProfilePicture = profilePic,
                Username = username,
                Accuracy = accuracy,
                CompletedQuizzes = completedQuizzes
            };

            // Act & Assert
            Assert.Equal(userId, entry.UserId);
            Assert.Equal(rank, entry.Rank);
            Assert.Equal(profilePic, entry.ProfilePicture);
            Assert.Equal(username, entry.Username);
            Assert.Equal(accuracy, entry.Accuracy);
            Assert.Equal(completedQuizzes, entry.CompletedQuizzes);
        }
    }
} 