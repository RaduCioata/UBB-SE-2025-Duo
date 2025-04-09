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
using Moq;
using Duo.Data;
using Duo.Helpers;
using Duo.Constants;

namespace TestsDuo2.Repositories
{
	public class UserRepositoryTests
	{
		private readonly Mock<IDataLink> _mockDataLink;
		private readonly UserRepository _userRepository;
		private readonly IDataLink _databaseConnection;

		private const string PROCEDURE_GET_USER_BY_USERNAME = "GetUserByUsername";
		private const string PROCEDURE_GET_USER_BY_EMAIL = "GetUserByEmail";
		private const string PROCEDURE_CREATE_USER = "CreateUser";
		private const string PROCEDURE_UPDATE_USER = "UpdateUser";
		private const string PARAM_USERNAME = "@Username";
		private const string PARAM_EMAIL = "@Email";
		private const string PARAM_USER_ID = "@UserId";

		public UserRepositoryTests()
		{
			_mockDataLink = new Mock<IDataLink>();
			_userRepository = new UserRepository(_mockDataLink.Object);
		}

		[Fact]
		public void Constructor_WithNullDataLink_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => new UserRepository(null));
		}

		[Fact]
		public void GetUserByUsername_WithInvalidUsername_ThrowsArgumentException()
		{
			// Arrange
			string invalidUsername = "";

			// Act & Assert
			var exception = Assert.Throws<ArgumentException>(() => _userRepository.GetUserByUsername(invalidUsername));
			Assert.Contains("Invalid username", exception.Message);
			Assert.Equal("username", exception.ParamName);
		}

		[Fact]
		public void GetUserByUsername_WithNonexistentUsername_ReturnsNull()
		{
			// Arrange
			string username = "nonexistent";
			var emptyDataTable = new DataTable();
			emptyDataTable.Columns.Add("UserId", typeof(int));
			emptyDataTable.Columns.Add("UserName", typeof(string));

			_mockDataLink.Setup(d => d.ExecuteReader(
				PROCEDURE_GET_USER_BY_USERNAME,
				It.Is<SqlParameter[]>(p => p[0].ParameterName == PARAM_USERNAME && (string)p[0].Value == username)
			)).Returns(emptyDataTable);

			// Act
			var result = _userRepository.GetUserByUsername(username);

			// Assert
			Assert.Null(result);
		}


		[Fact]
		public void GetUserByEmail_WithNullOrEmptyEmail_ThrowsArgumentException()
		{
			// Arrange & Act & Assert
			Assert.Throws<ArgumentException>(() => _userRepository.GetUserByEmail(null));
			Assert.Throws<ArgumentException>(() => _userRepository.GetUserByEmail(""));
			Assert.Throws<ArgumentException>(() => _userRepository.GetUserByEmail(" "));
		}

		[Fact]
		public void CreateUser_WithValidUser_ReturnsUserId()
		{
			// Arrange
			var newUser = new User
			{
				UserName = "newuser",
				Email = "new@example.com",
				Password = "password123",
				PrivacyStatus = true,
				OnlineStatus = false,
				DateJoined = DateTime.Now,
				TotalPoints = 0,
				CoursesCompleted = 0,
				QuizzesCompleted = 0,
				Streak = 0,
				Accuracy = 0
			};
			int expectedUserId = 1;

			_mockDataLink.Setup(d => d.ExecuteScalar<int>(
				PROCEDURE_CREATE_USER,
				It.IsAny<SqlParameter[]>()
			)).Returns(expectedUserId);

			// Act
			var result = _userRepository.CreateUser(newUser);

			// Assert
			Assert.Equal(expectedUserId, result);
			_mockDataLink.Verify(d => d.ExecuteScalar<int>(
				PROCEDURE_CREATE_USER,
				It.Is<SqlParameter[]>(p =>
					p[0].ParameterName == "@UserName" && (string)p[0].Value == newUser.UserName &&
					p[1].ParameterName == "@Email" && (string)p[1].Value == newUser.Email
				)
			), Moq.Times.Once);
		}

		[Fact]
		public void CreateUser_WithNullUser_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => _userRepository.CreateUser(null));
		}

		[Fact]
		public void CreateUser_WhenStoredProcedureReturnsNull_ReturnsZero()
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
			_mockDataLink.SetupExecuteScalarResponse<object>("CreateUser", parameters, null);

			// Act
			var result = _userRepository.CreateUser(user);

			// Assert
			Assert.Equal(0, result);
		}

		[Fact]
		public void UpdateUser_WithValidUser_UpdatesSuccessfully()
		{
			// Arrange
			var userToUpdate = new User
			{
				UserId = 1,
				UserName = "updateduser",
				Email = "updated@example.com",
				Password = "newpassword",
				PrivacyStatus = true,
				OnlineStatus = true,
				DateJoined = DateTime.Now,
				TotalPoints = 100,
				CoursesCompleted = 5,
				QuizzesCompleted = 10,
				Streak = 3,
				Accuracy = (decimal) 0.85
			};

			_mockDataLink.Setup(d => d.ExecuteNonQuery(
				PROCEDURE_UPDATE_USER,
				It.IsAny<SqlParameter[]>()
			));

			// Act
			_userRepository.UpdateUser(userToUpdate);

			// Assert
			_mockDataLink.Verify(d => d.ExecuteNonQuery(
				PROCEDURE_UPDATE_USER,
				It.Is<SqlParameter[]>(p =>
					p[0].ParameterName == PARAM_USER_ID && (int)p[0].Value == userToUpdate.UserId &&
					p[1].ParameterName == "@UserName" && (string)p[1].Value == userToUpdate.UserName
				)
			), Moq.Times.Once);
		}

		[Fact]
		public void UpdateUser_WithNullUser_ThrowsArgumentNullException()
		{
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => _userRepository.UpdateUser(null));
		}
	}
    // Add the following helper method to mock the behavior of SetupExecuteScalarResponse
    public static class MockExtensions
    {
        public static void SetupExecuteScalarResponse<T>(this Mock<IDataLink> mock, string storedProcedureName, SqlParameter[] parameters, T response)
        {
            mock.Setup(d => d.ExecuteScalar<T>(
                It.Is<string>(sp => sp == storedProcedureName),
                It.Is<SqlParameter[]>(p => ParametersMatch(p, parameters))
            )).Returns(response);
        }

        private static bool ParametersMatch(SqlParameter[] actual, SqlParameter[] expected)
        {
            if (actual == null && expected == null) return true;
            if (actual == null || expected == null || actual.Length != expected.Length) return false;

            for (int i = 0; i < actual.Length; i++)
            {
                if (actual[i].ParameterName != expected[i].ParameterName || !Equals(actual[i].Value, expected[i].Value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
