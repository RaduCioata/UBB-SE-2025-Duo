using Duo.Interfaces;
using Duo.Models;
using Duo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Duo.Tests.Services
{
    /// <summary>
    /// Tests for the <see cref="LoginService"/> class.
    /// </summary>
    [TestClass]
    public class LoginServiceTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private ILoginService loginService;

        /// <summary>
        /// Sets up the test environment before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            loginService = new LoginService(userRepositoryMock.Object);
        }

        /// <summary>
        /// Tests that AuthenticateUser calls ValidateCredentials on the repository.
        /// </summary>
        [TestMethod]
        public void AuthenticateUser_CallsRepositoryValidateCredentials()
        {
            // Arrange
            string username = "testUser";
            string password = "testPass";
            userRepositoryMock.Setup(repo => repo.ValidateCredentials(username, password))
                .Returns(true);

            // Act
            bool result = loginService.AuthenticateUser(username, password);

            // Assert
            Assert.IsTrue(result);
            userRepositoryMock.Verify(repo => repo.ValidateCredentials(username, password), Times.Once);
        }

        /// <summary>
        /// Tests that GetUserByCredentials returns null when authentication fails.
        /// </summary>
        [TestMethod]
        public void GetUserByCredentials_WhenAuthenticationFails_ReturnsNull()
        {
            // Arrange
            string username = "testUser";
            string password = "wrongPass";
            userRepositoryMock.Setup(repo => repo.ValidateCredentials(username, password))
                .Returns(false);
            userRepositoryMock.Setup(repo => repo.GetUserByCredentials(username, password))
                .Returns((User)null);

            // Act
            User user = loginService.GetUserByCredentials(username, password);

            // Assert
            Assert.IsNull(user);
            userRepositoryMock.Verify(repo => repo.ValidateCredentials(username, password), Times.Once);
            userRepositoryMock.Verify(repo => repo.GetUserByCredentials(username, password), Times.Once);
        }

        /// <summary>
        /// Tests that GetUserByCredentials returns user and updates online status when authentication succeeds.
        /// </summary>
        [TestMethod]
        public void GetUserByCredentials_WhenAuthenticationSucceeds_ReturnsUserAndUpdatesStatus()
        {
            // Arrange
            string username = "testUser";
            string password = "correctPass";
            var testUser = new User
            {
                UserId = 1,
                UserName = username,
                Password = password,
                OnlineStatus = false
            };

            userRepositoryMock.Setup(repo => repo.ValidateCredentials(username, password))
                .Returns(true);
            userRepositoryMock.Setup(repo => repo.GetUserByUsername(username))
                .Returns(testUser);
            userRepositoryMock.Setup(repo => repo.GetUserByCredentials(username, password))
                .Returns(testUser);

            // Act
            User user = loginService.GetUserByCredentials(username, password);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(username, user.UserName);
            userRepositoryMock.Verify(repo => repo.ValidateCredentials(username, password), Times.Once);
            userRepositoryMock.Verify(repo => repo.GetUserByUsername(username), Times.Once);
            userRepositoryMock.Verify(repo => repo.UpdateUser(It.Is<User>(u => u.OnlineStatus == true)), Times.Once);
            userRepositoryMock.Verify(repo => repo.GetUserByCredentials(username, password), Times.Once);
        }

        /// <summary>
        /// Tests that UpdateUserStatusOnLogout throws when user is null.
        /// </summary>
        [TestMethod]
        public void UpdateUserStatusOnLogout_WhenUserIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => loginService.UpdateUserStatusOnLogout(null));
        }

        /// <summary>
        /// Tests that UpdateUserStatusOnLogout updates user status to offline.
        /// </summary>
        [TestMethod]
        public void UpdateUserStatusOnLogout_SetsUserStatusToOffline()
        {
            // Arrange
            var testUser = new User
            {
                UserId = 1,
                UserName = "testUser",
                OnlineStatus = true,
                LastActivityDate = null
            };

            // Act
            loginService.UpdateUserStatusOnLogout(testUser);

            // Assert
            Assert.IsFalse(testUser.OnlineStatus);
            Assert.IsNotNull(testUser.LastActivityDate);
            userRepositoryMock.Verify(repo => repo.UpdateUser(It.Is<User>(u => 
                u.OnlineStatus == false && u.LastActivityDate != null)), Times.Once);
        }
    }
} 