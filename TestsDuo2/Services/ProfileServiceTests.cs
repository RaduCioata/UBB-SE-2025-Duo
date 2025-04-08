using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duo.Interfaces;
using Duo.Models;
using Duo.Services;
using DuolingoNou.Models;
using Moq;
using Xunit;

namespace TestsDuo2.Services
{
    public class ProfileServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly ProfileService _profileService;
        
        public ProfileServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _profileService = new ProfileService(_mockUserRepository.Object);
        }
        
        [Fact]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProfileService(null));
        }
        
        [Fact]
        public void CreateUser_CallsRepositoryMethod()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser" };
            
            // Act
            _profileService.CreateUser(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.CreateUser(user), Times.Once);
        }
        
        [Fact]
        public void UpdateUser_CallsRepositoryMethod()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser" };
            
            // Act
            _profileService.UpdateUser(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.UpdateUser(user), Times.Once);
        }
        
        [Fact]
        public void GetUserStats_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedUser = new User { UserId = userId, UserName = "testuser" };
            _mockUserRepository.Setup(r => r.GetUserStats(userId)).Returns(expectedUser);
            
            // Act
            var result = _profileService.GetUserStats(userId);
            
            // Assert
            Assert.Same(expectedUser, result);
            _mockUserRepository.Verify(r => r.GetUserStats(userId), Times.Once);
        }
        
        [Fact]
        public void GetUserAchievements_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "First Achievement" },
                new Achievement { Id = 2, Name = "Second Achievement" }
            };
            _mockUserRepository.Setup(r => r.GetUserAchievements(userId)).Returns(expectedAchievements);
            
            // Act
            var result = _profileService.GetUserAchievements(userId);
            
            // Assert
            Assert.Same(expectedAchievements, result);
            _mockUserRepository.Verify(r => r.GetUserAchievements(userId), Times.Once);
        }
        
        [Fact]
        public void AwardAchievements_NoQualifyingAchievements_DoesNotAwardAnyAchievements()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 5, QuizzesCompleted = 5, CoursesCompleted = 5 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>();
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
        
        [Fact]
        public void AwardAchievements_StreakAchievement_AwardsCorrectAchievement()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 15, QuizzesCompleted = 5, CoursesCompleted = 5 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>();
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 1), Times.Once);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 2), Times.Never);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 3), Times.Never);
        }
        
        [Fact]
        public void AwardAchievements_QuizzesAchievement_AwardsCorrectAchievement()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 5, QuizzesCompleted = 15, CoursesCompleted = 5 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>();
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 1), Times.Never);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 2), Times.Once);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 3), Times.Never);
        }
        
        [Fact]
        public void AwardAchievements_CoursesAchievement_AwardsCorrectAchievement()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 5, QuizzesCompleted = 5, CoursesCompleted = 15 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>();
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 1), Times.Never);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 2), Times.Never);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 3), Times.Once);
        }
        
        [Fact]
        public void AwardAchievements_MultipleQualifyingAchievements_AwardsAllQualifyingAchievements()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 15, QuizzesCompleted = 15, CoursesCompleted = 15 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>();
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 1), Times.Once);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 2), Times.Once);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 3), Times.Once);
        }
        
        [Fact]
        public void AwardAchievements_AlreadyAwardedAchievements_DoesNotAwardAgain()
        {
            // Arrange
            var user = new User { UserId = 1, UserName = "testuser", Streak = 15, QuizzesCompleted = 15, CoursesCompleted = 15 };
            var allAchievements = new List<Achievement> 
            { 
                new Achievement { Id = 1, Name = "10 Day Streak" },
                new Achievement { Id = 2, Name = "10 Quizzes Completed" },
                new Achievement { Id = 3, Name = "10 Courses Completed" }
            };
            var userAchievements = new List<Achievement>
            {
                new Achievement { Id = 1, Name = "10 Day Streak" }
            };
            
            _mockUserRepository.Setup(r => r.GetAllAchievements()).Returns(allAchievements);
            _mockUserRepository.Setup(r => r.GetUserAchievements(user.UserId)).Returns(userAchievements);
            
            // Act
            _profileService.AwardAchievements(user);
            
            // Assert
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 1), Times.Never);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 2), Times.Once);
            _mockUserRepository.Verify(r => r.AwardAchievement(user.UserId, 3), Times.Once);
        }
        
        [Theory]
        [InlineData("10 Day Streak", 10)]
        [InlineData("50 Day Streak", 50)]
        [InlineData("100 Day Streak", 100)]
        [InlineData("250 Day Streak", 250)]
        [InlineData("500 Day Streak", 500)]
        [InlineData("1000 Day Streak", 1000)]
        [InlineData("Unknown Achievement", 0)]
        public void GetAchievementThreshold_ReturnsCorrectThreshold(string achievementName, int expectedThreshold)
        {
            // Arrange
            // Using reflection to access the private method
            var method = typeof(ProfileService).GetMethod("GetAchievementThreshold", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Act
            var result = method.Invoke(_profileService, new object[] { achievementName });
            
            // Assert
            Assert.Equal(expectedThreshold, result);
        }
    }
} 