using System;
using Xunit;
using Duo.Models;

namespace TestsDuo2
{
    public class DuoTests
    {
        [Fact]
        public void UserModel_PropertiesTest()
        {
            // Testing the User model from Duo
            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "Password123",
                ProfileImage = "default.jpg"
            };

            Assert.Equal(1, user.UserId);
            Assert.Equal("TestUser", user.UserName);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("Password123", user.Password);
            Assert.Equal("default.jpg", user.ProfileImage);
        }
    }
} 