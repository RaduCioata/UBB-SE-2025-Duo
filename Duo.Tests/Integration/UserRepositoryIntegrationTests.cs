using Duo.Data;
using Duo.Interfaces;
using Duo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Duo.Models;

namespace Duo.Tests.Integration
{
    /// <summary>
    /// Integration tests for the <see cref="UserRepository"/> class.
    /// </summary>
    [TestClass]
    public class UserRepositoryIntegrationTests
    {
        private Mock<IDataLink> dataLinkMock;
        private IUserRepository userRepository;
        
        /// <summary>
        /// Sets up the test environment before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            dataLinkMock = new Mock<IDataLink>();
            userRepository = new UserRepository(dataLinkMock.Object);
        }
        
        /// <summary>
        /// Tests that GetUserByUsername returns null when no user is found.
        /// </summary>
        [TestMethod]
        public void GetUserByUsername_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            string username = "nonexistentUser";
            var emptyDataTable = new DataTable();
            
            dataLinkMock.Setup(dl => dl.ExecuteReader("GetUserByUsername", It.Is<SqlParameter[]>(
                parameters => parameters.Length == 1 && 
                             parameters[0].ParameterName == "@Username" && 
                             parameters[0].Value.ToString() == username)))
                .Returns(emptyDataTable);
            
            // Act
            var result = userRepository.GetUserByUsername(username);
            
            // Assert
            Assert.IsNull(result);
            dataLinkMock.Verify(dl => dl.ExecuteReader("GetUserByUsername", It.IsAny<SqlParameter[]>()), Times.Once);
        }
        
        /// <summary>
        /// Tests that GetUserByUsername returns a user when found.
        /// </summary>
        [TestMethod]
        public void GetUserByUsername_WhenUserExists_ReturnsUser()
        {
            // Arrange
            string username = "existingUser";
            var dataTable = CreateUserDataTable();
            
            var row = dataTable.NewRow();
            row["UserId"] = 1;
            row["UserName"] = username;
            row["Email"] = "user@example.com";
            row["Password"] = "password";
            row["PrivacyStatus"] = true;
            row["OnlineStatus"] = false;
            row["DateJoined"] = DateTime.Now.AddDays(-30);
            row["ProfileImage"] = "profile.jpg";
            row["TotalPoints"] = 100;
            row["CoursesCompleted"] = 5;
            row["QuizzesCompleted"] = 10;
            row["Streak"] = 7;
            row["LastActivityDate"] = DateTime.Now.AddDays(-1);
            row["Accuracy"] = 85.5m;
            dataTable.Rows.Add(row);
            
            dataLinkMock.Setup(dl => dl.ExecuteReader("GetUserByUsername", It.Is<SqlParameter[]>(
                parameters => parameters.Length == 1 && 
                             parameters[0].ParameterName == "@Username" && 
                             parameters[0].Value.ToString() == username)))
                .Returns(dataTable);
            
            // Act
            var result = userRepository.GetUserByUsername(username);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.UserId);
            Assert.AreEqual(username, result.UserName);
            Assert.AreEqual("user@example.com", result.Email);
            dataLinkMock.Verify(dl => dl.ExecuteReader("GetUserByUsername", It.IsAny<SqlParameter[]>()), Times.Once);
        }
        
        /// <summary>
        /// Tests that ValidateCredentials returns true when credentials are valid.
        /// </summary>
        [TestMethod]
        public void ValidateCredentials_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "validUser";
            string password = "validPass";
            var dataTable = CreateUserDataTable();
            
            var row = dataTable.NewRow();
            row["UserId"] = 1;
            row["UserName"] = username;
            row["Email"] = "user@example.com";
            row["Password"] = password;
            row["PrivacyStatus"] = true;
            row["OnlineStatus"] = false;
            row["DateJoined"] = DateTime.Now.AddDays(-30);
            row["ProfileImage"] = "profile.jpg";
            row["TotalPoints"] = 100;
            row["CoursesCompleted"] = 5;
            row["QuizzesCompleted"] = 10;
            row["Streak"] = 7;
            row["LastActivityDate"] = DateTime.Now.AddDays(-1);
            row["Accuracy"] = 85.5m;
            dataTable.Rows.Add(row);
            
            dataLinkMock.Setup(dl => dl.ExecuteReader("GetUserByUsername", It.Is<SqlParameter[]>(
                parameters => parameters.Length == 1 && 
                             parameters[0].ParameterName == "@Username" && 
                             parameters[0].Value.ToString() == username)))
                .Returns(dataTable);
            
            // Act
            var result = userRepository.ValidateCredentials(username, password);
            
            // Assert
            Assert.IsTrue(result);
        }
        
        /// <summary>
        /// Creates a DataTable with the structure required for User data.
        /// </summary>
        /// <returns>A DataTable with the User schema.</returns>
        private DataTable CreateUserDataTable()
        {
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
            
            return dataTable;
        }
    }
} 