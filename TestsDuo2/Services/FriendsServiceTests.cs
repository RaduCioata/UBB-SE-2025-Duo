using System;
using System.Collections.Generic;
using Duo.Models;
using Duo.Repositories;
using Duo.Services;
using Moq;
using Xunit;

namespace TestsDuo2.Services
{
    public class FriendsServiceTests
    {
        private readonly Mock<ListFriendsRepository> _mockFriendRepository;
        private readonly FriendsService _friendsService;
        
        public FriendsServiceTests()
        {
            _mockFriendRepository = new Mock<ListFriendsRepository>();
            _friendsService = new FriendsService(_mockFriendRepository.Object);
        }
        
        [Fact]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FriendsService(null));
        }
        
        [Fact]
        public void GetFriends_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedFriends = new List<User> { new User { UserId = 2 }, new User { UserId = 3 } };
            _mockFriendRepository.Setup(r => r.GetFriends(userId)).Returns(expectedFriends);
            
            // Act
            var result = _friendsService.GetFriends(userId);
            
            // Assert
            Assert.Same(expectedFriends, result);
            _mockFriendRepository.Verify(r => r.GetFriends(userId), Times.Once);
        }
        
        [Fact]
        public void SortFriendsByName_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedSortedFriends = new List<User> 
            { 
                new User { UserId = 2, UserName = "Alice" }, 
                new User { UserId = 3, UserName = "Bob" } 
            };
            _mockFriendRepository.Setup(r => r.SortFriendsByName(userId)).Returns(expectedSortedFriends);
            
            // Act
            var result = _friendsService.SortFriendsByName(userId);
            
            // Assert
            Assert.Same(expectedSortedFriends, result);
            _mockFriendRepository.Verify(r => r.SortFriendsByName(userId), Times.Once);
        }
        
        [Fact]
        public void SortFriendsByDateAdded_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedSortedFriends = new List<User> 
            { 
                new User { UserId = 2, DateJoined = DateTime.Now.AddDays(-10) }, 
                new User { UserId = 3, DateJoined = DateTime.Now.AddDays(-5) } 
            };
            _mockFriendRepository.Setup(r => r.SortFriendsByDateAdded(userId)).Returns(expectedSortedFriends);
            
            // Act
            var result = _friendsService.SortFriendsByDateAdded(userId);
            
            // Assert
            Assert.Same(expectedSortedFriends, result);
            _mockFriendRepository.Verify(r => r.SortFriendsByDateAdded(userId), Times.Once);
        }
        
        [Fact]
        public void SortFriendsByOnlineStatus_CallsRepositoryMethod()
        {
            // Arrange
            int userId = 1;
            var expectedSortedFriends = new List<User> 
            { 
                new User { UserId = 2, OnlineStatus = true }, 
                new User { UserId = 3, OnlineStatus = false } 
            };
            _mockFriendRepository.Setup(r => r.SortFriendsByOnlineStatus(userId)).Returns(expectedSortedFriends);
            
            // Act
            var result = _friendsService.SortFriendsByOnlineStatus(userId);
            
            // Assert
            Assert.Same(expectedSortedFriends, result);
            _mockFriendRepository.Verify(r => r.SortFriendsByOnlineStatus(userId), Times.Once);
        }
    }
} 