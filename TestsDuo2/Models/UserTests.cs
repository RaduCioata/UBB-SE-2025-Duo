using System;
using Xunit;
using DuoModels;

namespace TestsDuo2.Models
{
    public class UserTests
    {
        [Fact]
        public void User_DefaultProperties_AreSetCorrectly()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(0, user.UserId);
            Assert.Equal(string.Empty, user.UserName);
            Assert.Equal(string.Empty, user.Email);
            Assert.Equal(string.Empty, user.Password);
            Assert.Equal("default.jpg", user.ProfileImage);
            Assert.False(user.PrivacyStatus);
            Assert.False(user.OnlineStatus);
            Assert.Equal(0, user.TotalPoints);
            Assert.Equal(0, user.CoursesCompleted);
            Assert.Equal(0, user.QuizzesCompleted);
            Assert.Equal(0, user.Streak);
            Assert.Equal(0.00m, user.Accuracy);
            Assert.Null(user.LastActivityDate);
        }

        [Fact]
        public void User_SetProperties_ReturnCorrectValues()
        {
            // Arrange
            var user = new User
            {
                UserId = 42,
                UserName = "JohnDoe",
                Email = "john@example.com",
                Password = "securePassword123",
                ProfileImage = "profile.jpg",
                PrivacyStatus = true,
                OnlineStatus = true,
                TotalPoints = 1000,
                CoursesCompleted = 5,
                QuizzesCompleted = 25,
                Streak = 7,
                Accuracy = 92.5m,
                LastActivityDate = new DateTime(2023, 12, 31, 23, 59, 59),
                DateJoined = new DateTime(2023, 1, 1)
            };

            // Act & Assert
            Assert.Equal(42, user.UserId);
            Assert.Equal("JohnDoe", user.UserName);
            Assert.Equal("john@example.com", user.Email);
            Assert.Equal("securePassword123", user.Password);
            Assert.Equal("profile.jpg", user.ProfileImage);
            Assert.True(user.PrivacyStatus);
            Assert.True(user.OnlineStatus);
            Assert.Equal(1000, user.TotalPoints);
            Assert.Equal(5, user.CoursesCompleted);
            Assert.Equal(25, user.QuizzesCompleted);
            Assert.Equal(7, user.Streak);
            Assert.Equal(92.5m, user.Accuracy);
            Assert.Equal(new DateTime(2023, 12, 31, 23, 59, 59), user.LastActivityDate);
            Assert.Equal(new DateTime(2023, 1, 1), user.DateJoined);
        }

        [Fact]
        public void OnlineStatusText_WhenOnline_ReturnsActive()
        {
            // Arrange
            var user = new User { OnlineStatus = true };

            // Act
            string statusText = user.OnlineStatusText;

            // Assert
            Assert.Equal("Active", statusText);
        }

        [Fact]
        public void OnlineStatusText_WhenOffline_ReturnsNotActive()
        {
            // Arrange
            var user = new User { OnlineStatus = false };

            // Act
            string statusText = user.OnlineStatusText;

            // Assert
            Assert.Equal("Not Active", statusText);
        }

        [Fact]
        public void GetLastSeenText_WhenUserIsActive_ReturnsActiveNow()
        {
            // Arrange
            var user = new User { OnlineStatus = true };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Equal("Active Now", lastSeenText);
        }

        [Fact]
        public void GetLastSeenText_WithoutLastActivityDate_ReturnsLongTimeAgo()
        {
            // Arrange
            var user = new User { OnlineStatus = false, LastActivityDate = null };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Equal("Last seen a long time ago", lastSeenText);
        }

        [Fact]
        public void GetLastSeenText_WithLastActivityDateLessThanOneMinute_ReturnsLessThanMinute()
        {
            // Arrange
            var user = new User
            {
                OnlineStatus = false,
                LastActivityDate = DateTime.Now.AddSeconds(-30)
            };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Equal("Less than a minute ago", lastSeenText);
        }

        [Fact]
        public void GetLastSeenText_WithLastActivityDateLessThanOneHour_ReturnsMinutesAgo()
        {
            // Arrange
            var user = new User
            {
                OnlineStatus = false,
                LastActivityDate = DateTime.Now.AddMinutes(-10)
            };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Contains("minutes ago", lastSeenText);
            Assert.StartsWith("10", lastSeenText);
        }

        [Fact]
        public void GetLastSeenText_WithLastActivityDateLessThanOneDay_ReturnsHoursAgo()
        {
            // Arrange
            var user = new User
            {
                OnlineStatus = false,
                LastActivityDate = DateTime.Now.AddHours(-3)
            };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Contains("hours ago", lastSeenText);
            Assert.StartsWith("3", lastSeenText);
        }

        [Fact]
        public void GetLastSeenText_WithLastActivityDateMoreThanOneDay_ReturnsDaysAgo()
        {
            // Arrange
            var user = new User
            {
                OnlineStatus = false,
                LastActivityDate = DateTime.Now.AddDays(-7)
            };

            // Act
            string lastSeenText = user.GetLastSeenText;

            // Assert
            Assert.Contains("days ago", lastSeenText);
            Assert.StartsWith("7", lastSeenText);
        }
    }
} 