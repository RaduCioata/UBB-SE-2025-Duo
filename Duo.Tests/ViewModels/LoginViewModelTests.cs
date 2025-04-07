using Duo.Interfaces;
using Duo.Models;
using Duo.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Duo.Tests.ViewModels
{
    /// <summary>
    /// Tests for the <see cref="LoginViewModel"/> class.
    /// </summary>
    [TestClass]
    public class LoginViewModelTests
    {
        private Mock<ILoginService> loginServiceMock;
        private LoginViewModel viewModel;

        /// <summary>
        /// Sets up the test environment before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            loginServiceMock = new Mock<ILoginService>();
            viewModel = new LoginViewModel(loginServiceMock.Object);
        }

        /// <summary>
        /// Tests that the constructor throws when loginService is null.
        /// </summary>
        [TestMethod]
        public void Constructor_WhenLoginServiceIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new LoginViewModel(null));
        }

        /// <summary>
        /// Tests that AttemptLogin sets LoginStatus to false when username is empty.
        /// </summary>
        [TestMethod]
        public void AttemptLogin_WhenUsernameIsEmpty_SetsLoginStatusToFalse()
        {
            // Act
            viewModel.AttemptLogin(string.Empty, "password");

            // Assert
            Assert.IsFalse(viewModel.LoginStatus);
            loginServiceMock.Verify(service => service.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Tests that AttemptLogin sets LoginStatus to false when password is empty.
        /// </summary>
        [TestMethod]
        public void AttemptLogin_WhenPasswordIsEmpty_SetsLoginStatusToFalse()
        {
            // Act
            viewModel.AttemptLogin("username", string.Empty);

            // Assert
            Assert.IsFalse(viewModel.LoginStatus);
            loginServiceMock.Verify(service => service.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>
        /// Tests that AttemptLogin sets LoginStatus to false when authentication fails.
        /// </summary>
        [TestMethod]
        public void AttemptLogin_WhenAuthenticationFails_SetsLoginStatusToFalse()
        {
            // Arrange
            string username = "testUser";
            string password = "wrongPass";
            loginServiceMock.Setup(service => service.GetUserByCredentials(username, password))
                .Returns((User)null);

            // Act
            viewModel.AttemptLogin(username, password);

            // Assert
            Assert.IsFalse(viewModel.LoginStatus);
            Assert.IsNull(viewModel.LoggedInUser);
            loginServiceMock.Verify(service => service.GetUserByCredentials(username, password), Times.Once);
        }

        /// <summary>
        /// Tests that AttemptLogin sets LoginStatus to true and LoggedInUser to the returned user when authentication succeeds.
        /// </summary>
        [TestMethod]
        public void AttemptLogin_WhenAuthenticationSucceeds_SetsLoginStatusToTrueAndSetsLoggedInUser()
        {
            // Arrange
            string username = "testUser";
            string password = "correctPass";
            var testUser = new User
            {
                UserId = 1,
                UserName = username,
                Password = password
            };

            loginServiceMock.Setup(service => service.GetUserByCredentials(username, password))
                .Returns(testUser);

            // Act
            viewModel.AttemptLogin(username, password);

            // Assert
            Assert.IsTrue(viewModel.LoginStatus);
            Assert.IsNotNull(viewModel.LoggedInUser);
            Assert.AreEqual(testUser, viewModel.LoggedInUser);
            loginServiceMock.Verify(service => service.GetUserByCredentials(username, password), Times.Once);
        }
    }
} 