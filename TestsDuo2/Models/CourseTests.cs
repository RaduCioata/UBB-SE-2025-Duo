using System;
using Xunit;
using Duo.Models;

namespace TestsDuo2.Models
{
    public class CourseTests
    {
        [Fact]
        public void Course_SetProperties_ReturnCorrectValues()
        {
            // Arrange
            var course = new Course
            {
                Id = 42,
                Name = "Spanish for Beginners",
                TotalNumOfLessons = 25
            };

            // Act & Assert
            Assert.Equal(42, course.Id);
            Assert.Equal("Spanish for Beginners", course.Name);
            Assert.Equal(25, course.TotalNumOfLessons);
        }
        
        [Theory]
        [InlineData(1, "English", 10)]
        [InlineData(2, "French", 20)]
        [InlineData(3, "German", 15)]
        [InlineData(4, "Japanese", 30)]
        public void Course_SetVariousValues_ReturnCorrectValues(int id, string name, int totalLessons)
        {
            // Arrange
            var course = new Course
            {
                Id = id,
                Name = name,
                TotalNumOfLessons = totalLessons
            };

            // Act & Assert
            Assert.Equal(id, course.Id);
            Assert.Equal(name, course.Name);
            Assert.Equal(totalLessons, course.TotalNumOfLessons);
        }
    }
} 