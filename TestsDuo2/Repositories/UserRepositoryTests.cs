using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Duo.Interfaces;
using Duo.Models;
using Duo.Repositories;
using Microsoft.Data.SqlClient;
using TestsDuo2.Mocks;
using TestsDuo2.TestHelpers;
using Xunit;
using DuoUser = Duo.Models.User;

namespace TestsDuo2.Repositories
{
    public class UserRepositoryTests
    {
        private readonly MockDataLink mockDataLink;
        private readonly UserRepository userRepository;

        public UserRepositoryTests()
        {
            mockDataLink = new MockDataLink();
            userRepository = new UserRepository(mockDataLink);
        }
        
        [Fact]
        public void Constructor_WithNullDataLink_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UserRepository(null));
        }

        [Fact]
        public void GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, "testuser", "test@example.com", "password", false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", parameters, dataTable);

            // Act
            var user = userRepository.GetUserByUsername(username);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
            Assert.Equal("testuser", user.UserName);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("password", user.Password);
        }

        [Fact]
        public void GetUserByUsername_WithNullOrEmptyUsername_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByUsername(null));
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByUsername(""));
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByUsername(" "));
        }

        [Fact]
        public void GetUserByUsername_WithNonExistentUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", parameters, dataTable);

            // Act
            var user = userRepository.GetUserByUsername(username);

            // Assert
            Assert.Null(user);
        }
        
        [Fact]
        public void GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, "testuser", "test@example.com", "password", false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByEmail", parameters, dataTable);

            // Act
            var user = userRepository.GetUserByEmail(email);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
            Assert.Equal("testuser", user.UserName);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("password", user.Password);
        }

        [Fact]
        public void GetUserByEmail_WithNullOrEmptyEmail_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByEmail(null));
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByEmail(""));
            Assert.Throws<ArgumentException>(() => userRepository.GetUserByEmail(" "));
        }

        [Fact]
        public void GetUserByEmail_WithNonExistentEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("Email", typeof(string));
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByEmail", parameters, dataTable);

            // Act
            var user = userRepository.GetUserByEmail(email);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void CreateUser_WithValidUser_ReturnsUserId()
        {
            // Arrange
            var newUser = UserFactory.CreateUser(
                username: "newuser",
                email: "new@example.com",
                password: "password123",
                profileImage: "newprofile.jpg"
            );
            
            int expectedUserId = 42;
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", newUser.UserName),
                new SqlParameter("@Email", newUser.Email),
                new SqlParameter("@Password", newUser.Password),
                new SqlParameter("@PrivacyStatus", newUser.PrivacyStatus),
                new SqlParameter("@OnlineStatus", newUser.OnlineStatus),
                new SqlParameter("@DateJoined", newUser.DateJoined),
                new SqlParameter("@ProfileImage", newUser.ProfileImage),
                new SqlParameter("@TotalPoints", newUser.TotalPoints),
                new SqlParameter("@CoursesCompleted", newUser.CoursesCompleted),
                new SqlParameter("@QuizzesCompleted", newUser.QuizzesCompleted),
                new SqlParameter("@Streak", newUser.Streak),
                new SqlParameter("@LastActivityDate", newUser.LastActivityDate),
                new SqlParameter("@Accuracy", newUser.Accuracy)
            };
            mockDataLink.SetupExecuteScalarResponse("CreateUser", parameters, expectedUserId);

            // Act
            var userId = userRepository.CreateUser(newUser);

            // Assert
            Assert.Equal(expectedUserId, userId);
        }

        [Fact]
        public void CreateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.CreateUser(null));
        }
        
        [Fact]
        public void CreateUser_WhenStoredProcedureReturnsNull_ReturnsNegativeOne()
        {
            // Arrange
            var user = UserFactory.CreateUser(
                username: "testuser",
                email: "test@example.com"
            );
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@PrivacyStatus", user.PrivacyStatus),
                new SqlParameter("@OnlineStatus", user.OnlineStatus),
                new SqlParameter("@DateJoined", user.DateJoined),
                new SqlParameter("@ProfileImage", user.ProfileImage),
                new SqlParameter("@TotalPoints", user.TotalPoints),
                new SqlParameter("@CoursesCompleted", user.CoursesCompleted),
                new SqlParameter("@QuizzesCompleted", user.QuizzesCompleted),
                new SqlParameter("@Streak", user.Streak),
                new SqlParameter("@LastActivityDate", user.LastActivityDate),
                new SqlParameter("@Accuracy", user.Accuracy)
            };
            mockDataLink.SetupExecuteScalarResponse<object>("CreateUser", parameters, null);

            // Act
            var result = userRepository.CreateUser(user);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void UpdateUser_WithValidUser_CallsExecuteNonQuery()
        {
            // Arrange
            var user = UserFactory.CreateUser(
                id: 1,
                username: "updateduser",
                email: "updated@example.com",
                password: "updatedpassword",
                privacyStatus: true,
                onlineStatus: false,
                profileImage: "updated.jpg",
                totalPoints: 200,
                coursesCompleted: 10,
                quizzesCompleted: 20,
                streak: 5,
                accuracy: 97.5m
            );
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", user.UserId),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@PrivacyStatus", user.PrivacyStatus),
                new SqlParameter("@OnlineStatus", user.OnlineStatus),
                new SqlParameter("@DateJoined", user.DateJoined),
                new SqlParameter("@ProfileImage", user.ProfileImage),
                new SqlParameter("@TotalPoints", user.TotalPoints),
                new SqlParameter("@CoursesCompleted", user.CoursesCompleted),
                new SqlParameter("@QuizzesCompleted", user.QuizzesCompleted),
                new SqlParameter("@Streak", user.Streak),
                new SqlParameter("@LastActivityDate", user.LastActivityDate),
                new SqlParameter("@Accuracy", user.Accuracy)
            };
            mockDataLink.SetupExecuteNonQueryResponse("UpdateUser", parameters, 1);

            // Act
            userRepository.UpdateUser(user);

            // Assert
            mockDataLink.VerifyExecuteNonQuery("UpdateUser", Times.Once);
        }

        [Fact]
        public void UpdateUser_WithNullUser_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.UpdateUser(null));
        }

        [Fact]
        public void ValidateCredentials_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var username = "testuser";
            var password = "correctpassword";
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, username, "test@example.com", password, false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] usernameParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", usernameParameters, dataTable);

            // Act
            var result = userRepository.ValidateCredentials(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCredentials_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var username = "testuser";
            var correctPassword = "correctpassword";
            var wrongPassword = "wrongpassword";
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, username, "test@example.com", correctPassword, false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] usernameParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", usernameParameters, dataTable);

            // Act
            var result = userRepository.ValidateCredentials(username, wrongPassword);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCredentials_WithNonExistentUsername_ReturnsFalse()
        {
            // Arrange
            var username = "nonexistent";
            var password = "anypassword";
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            
            SqlParameter[] usernameParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", usernameParameters, dataTable);

            // Act
            var result = userRepository.ValidateCredentials(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetUserByCredentials_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var password = "correctpassword";
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, username, "test@example.com", password, false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] usernameParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", usernameParameters, dataTable);

            // Act
            var user = userRepository.GetUserByCredentials(username, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(1, user.UserId);
            Assert.Equal(username, user.UserName);
            Assert.Equal("test@example.com", user.Email);
        }

        [Fact]
        public void GetUserByCredentials_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var username = "testuser";
            var correctPassword = "correctpassword";
            var wrongPassword = "wrongpassword";
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            dataTable.Rows.Add(1, username, "test@example.com", correctPassword, false, true, DateTime.Now, "profile.jpg", 100, 5, 10, 3, DateTime.Now, 95.5m);
            
            SqlParameter[] usernameParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserByUsername", usernameParameters, dataTable);

            // Act
            var user = userRepository.GetUserByCredentials(username, wrongPassword);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void GetTopUsersByCompletedQuizzes_ReturnsLeaderboardEntries()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            
            dataTable.Rows.Add(1, "user1", 50, 98.5m, "profile1.jpg");
            dataTable.Rows.Add(2, "user2", 45, 97.0m, "profile2.jpg");
            dataTable.Rows.Add(3, "user3", 40, 95.5m, "profile3.jpg");
            
            mockDataLink.SetupExecuteReaderResponse("GetTopUsersByCompletedQuizzes", null, dataTable);

            // Act
            var leaderboard = userRepository.GetTopUsersByCompletedQuizzes();

            // Assert
            Assert.NotNull(leaderboard);
            Assert.Equal(3, leaderboard.Count);
            
            // Check first entry
            Assert.Equal(1, leaderboard[0].Rank);
            Assert.Equal(1, leaderboard[0].UserId);
            Assert.Equal("user1", leaderboard[0].Username);
            Assert.Equal(50, leaderboard[0].CompletedQuizzes);
            Assert.Equal(98.5m, leaderboard[0].Accuracy);
            
            // Check ranks are assigned correctly
            Assert.Equal(1, leaderboard[0].Rank);
            Assert.Equal(2, leaderboard[1].Rank);
            Assert.Equal(3, leaderboard[2].Rank);
        }

        [Fact]
        public void GetTopUsersByCompletedQuizzes_WithEmptyDataTable_ReturnsEmptyList()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            
            mockDataLink.SetupExecuteReaderResponse("GetTopUsersByCompletedQuizzes", null, dataTable);

            // Act
            var leaderboard = userRepository.GetTopUsersByCompletedQuizzes();

            // Assert
            Assert.NotNull(leaderboard);
            Assert.Empty(leaderboard);
        }

        [Fact]
        public void GetTopUsersByAccuracy_ReturnsLeaderboardEntries()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            
            dataTable.Rows.Add(1, "user1", 30, 99.5m, "profile1.jpg");
            dataTable.Rows.Add(2, "user2", 25, 98.0m, "profile2.jpg");
            dataTable.Rows.Add(3, "user3", 20, 97.5m, "profile3.jpg");
            
            mockDataLink.SetupExecuteReaderResponse("GetTopUsersByAccuracy", null, dataTable);

            // Act
            var leaderboard = userRepository.GetTopUsersByAccuracy();

            // Assert
            Assert.NotNull(leaderboard);
            Assert.Equal(3, leaderboard.Count);
            
            // Check first entry
            Assert.Equal(1, leaderboard[0].Rank);
            Assert.Equal(1, leaderboard[0].UserId);
            Assert.Equal("user1", leaderboard[0].Username);
            Assert.Equal(30, leaderboard[0].CompletedQuizzes);
            Assert.Equal(99.5m, leaderboard[0].Accuracy);
            
            // Check ranks are assigned correctly
            Assert.Equal(1, leaderboard[0].Rank);
            Assert.Equal(2, leaderboard[1].Rank);
            Assert.Equal(3, leaderboard[2].Rank);
        }

        [Fact]
        public void GetTopUsersByAccuracy_WithEmptyDataTable_ReturnsEmptyList()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            
            mockDataLink.SetupExecuteReaderResponse("GetTopUsersByAccuracy", null, dataTable);

            // Act
            var leaderboard = userRepository.GetTopUsersByAccuracy();

            // Assert
            Assert.NotNull(leaderboard);
            Assert.Empty(leaderboard);
        }

        
        
        [Fact]
        public void GetUserStats_WithNonExistentUserId_ReturnsNull()
        {
            // Arrange
            int userId = 999;
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            
            SqlParameter[] userIdParameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserStats", userIdParameters, dataTable);

            // Act
            var user = userRepository.GetUserStats(userId);

            // Assert
            Assert.Null(user);
        }
        
        [Fact]
        public void GetAllAchievements_ReturnsListOfAchievements()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Rarity", typeof(string));
            dataTable.Columns.Add("AwardedDate", typeof(DateTime));
            
            var now = DateTime.Now;
            dataTable.Rows.Add(1, "First Quiz", "Complete your first quiz", "Common", DBNull.Value);
            dataTable.Rows.Add(2, "Perfect Score", "Get 100% on a quiz", "Rare", DBNull.Value);
            dataTable.Rows.Add(3, "Streak Master", "Maintain a 7-day streak", "Epic", DBNull.Value);
            
            mockDataLink.SetupExecuteReaderResponse("GetAllAchievements", null, dataTable);

            // Act
            var achievements = userRepository.GetAllAchievements();

            // Assert
            Assert.NotNull(achievements);
            Assert.Equal(3, achievements.Count);
            
            Assert.Equal(1, achievements[0].Id);
            Assert.Equal("First Quiz", achievements[0].Name);
            Assert.Equal("Complete your first quiz", achievements[0].Description);
            Assert.Equal("Common", achievements[0].RarityLevel);
            Assert.Equal(default(DateTime), achievements[0].AchievementUnlockDate);
            
            Assert.Equal(2, achievements[1].Id);
            Assert.Equal("Perfect Score", achievements[1].Name);
        }
        
        [Fact]
        public void GetUserAchievements_ReturnsAchievementsForUser()
        {
            // Arrange
            int userId = 1;
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("AchievementId", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Rarity", typeof(string));
            dataTable.Columns.Add("AwardedDate", typeof(DateTime));
            
            var now = DateTime.Now;
            dataTable.Rows.Add(1, "First Quiz", "Complete your first quiz", "Common", now.AddDays(-10));
            dataTable.Rows.Add(2, "Perfect Score", "Get 100% on a quiz", "Rare", now.AddDays(-5));
            
            SqlParameter[] userIdParameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
            mockDataLink.SetupExecuteReaderResponse("GetUserAchievements", userIdParameters, dataTable);

            // Act
            var achievements = userRepository.GetUserAchievements(userId);

            // Assert
            Assert.NotNull(achievements);
            Assert.Equal(2, achievements.Count);
            
            Assert.Equal(1, achievements[0].Id);
            Assert.Equal("First Quiz", achievements[0].Name);
            Assert.Equal("Complete your first quiz", achievements[0].Description);
            Assert.Equal("Common", achievements[0].RarityLevel);
            Assert.True(achievements[0].AchievementUnlockDate > default(DateTime));
            
            Assert.Equal(2, achievements[1].Id);
            Assert.Equal("Perfect Score", achievements[1].Name);
            Assert.True(achievements[1].AchievementUnlockDate > default(DateTime));
        }
        
        [Fact]
        public void AwardAchievement_CallsExecuteNonQuery()
        {
            // Arrange
            int userId = 1;
            int achievementId = 2;
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@AchievementId", achievementId),
                new SqlParameter("@AwardedDate", DateTime.Now)
            };
            mockDataLink.SetupExecuteNonQueryResponse("AwardAchievement", parameters, 1);

            // Act
            userRepository.AwardAchievement(userId, achievementId);

            // Assert
            mockDataLink.VerifyExecuteNonQuery("AwardAchievement", Times.Once);
        }
        
        [Fact]
        public void GetFriends_ReturnsListOfUsers()
        {
            // Arrange
            int userId = 1;
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("PrivacyStatus", typeof(bool));
            dataTable.Columns.Add("OnlineStatus", typeof(bool));
            dataTable.Columns.Add("DateJoined", typeof(DateTime));
            dataTable.Columns.Add("ProfileImage", typeof(string));
            dataTable.Columns.Add("TotalPoints", typeof(int));
            dataTable.Columns.Add("CoursesCompleted", typeof(int));
            dataTable.Columns.Add("QuizzesCompleted", typeof(int));
            dataTable.Columns.Add("Streak", typeof(int));
            dataTable.Columns.Add("LastActivityDate", typeof(DateTime));
            dataTable.Columns.Add("Accuracy", typeof(decimal));
            
            var now = DateTime.Now;
            dataTable.Rows.Add(2, "friend1", "friend1@example.com", "password", false, true, 
                now.AddDays(-30), "profile1.jpg", 500, 3, 15, 4, now, 92.5m);
            dataTable.Rows.Add(3, "friend2", "friend2@example.com", "password", false, false, 
                now.AddDays(-45), "profile2.jpg", 700, 4, 18, 0, now.AddDays(-1), 90.0m);
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
            mockDataLink.SetupExecuteReaderResponse("GetFriends", parameters, dataTable);

            // Act
            var friends = userRepository.GetFriends(userId);

            // Assert
            Assert.NotNull(friends);
            Assert.Equal(2, friends.Count);
            
            Assert.Equal(2, friends[0].UserId);
            Assert.Equal("friend1", friends[0].UserName);
            Assert.True(friends[0].OnlineStatus);
            
            Assert.Equal(3, friends[1].UserId);
            Assert.Equal("friend2", friends[1].UserName);
            Assert.False(friends[1].OnlineStatus);
        }
        
        [Fact]
        public void GetFriends_WithNoFriends_ReturnsEmptyList()
        {
            // Arrange
            int userId = 1;
            
            var dataTable = new DataTable();
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
            mockDataLink.SetupExecuteReaderResponse("GetFriends", parameters, dataTable);

            // Act
            var friends = userRepository.GetFriends(userId);

            // Assert
            Assert.NotNull(friends);
            Assert.Empty(friends);
        }
    }
} 