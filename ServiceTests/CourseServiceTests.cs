using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuolingoNou.Services;
using Xunit;

namespace ServiceTests
{
    public class CourseServiceTests
    {
        private readonly MockCourseService _courseService;
        
        public CourseServiceTests()
        {
            _courseService = new MockCourseService();
        }
        
        [Fact]
        public async Task GetEnrolledCoursesAsync_ReturnsNonEmptyList()
        {
            // Act
            var courses = await _courseService.GetEnrolledCoursesAsync();
            
            // Assert
            Assert.NotNull(courses);
            Assert.NotEmpty(courses);
        }
        
        [Fact]
        public async Task GetEnrolledCoursesAsync_ReturnsCoursesWithValidProperties()
        {
            // Act
            var courses = await _courseService.GetEnrolledCoursesAsync();
            var firstCourse = courses.FirstOrDefault();
            
            // Assert
            Assert.NotNull(firstCourse);
            Assert.NotEmpty(firstCourse.Id);
            Assert.NotEmpty(firstCourse.Name);
            Assert.NotEmpty(firstCourse.Language);
            Assert.InRange(firstCourse.CompletionPercentage, 0.0, 1.0);
            Assert.InRange(firstCourse.EnrollmentDate, DateTime.Now.AddDays(-365), DateTime.Now);
        }
        
        [Fact]
        public async Task GetEnrolledCoursesAsync_GeneratesDifferentCourseEachTime()
        {
            // Act
            var firstResult = await _courseService.GetEnrolledCoursesAsync();
            var secondResult = await _courseService.GetEnrolledCoursesAsync();
            
            // We'll extract the IDs to compare the course sets
            var firstCourseIds = firstResult.Select(c => c.Id).ToList();
            var secondCourseIds = secondResult.Select(c => c.Id).ToList();
            
            // Assert
            // Verify either the count or contents are different (since this is random generation)
            bool hasDifferences = firstCourseIds.Count != secondCourseIds.Count ||
                                 !firstCourseIds.All(id => secondCourseIds.Contains(id));
            
            Assert.True(hasDifferences, "Mock course service should generate different data on each call");
        }
    }
} 