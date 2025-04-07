using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models;
using DuolingoNou.Models;

namespace DuolingoNou.Services
{
    public interface ICourseService
    {
        Task<List<MyCourse>> GetEnrolledCoursesAsync();
    }
    public class MockCourseService : ICourseService
    {
        private static readonly string[] ProgrammingLanguages = new[]
        {
            "Python", "JavaScript", "Java", "C#", "C++",
            "Ruby", "Swift", "Kotlin", "Go", "Rust",
            "TypeScript", "PHP", "Scala", "Dart", "R"
        };

        private static readonly string[] CourseTypes = new[]
        {
            "Beginner", "Intermediate", "Advanced", "Professional",
            "Full Stack", "Web Development", "Mobile Development",
            "Data Science", "Machine Learning", "DevOps"
        };

        public async Task<List<MyCourse>> GetEnrolledCoursesAsync()
        {
            await Task.Delay(100); // Simulate async operation
            var random = new Random();

            // Generate 10-20 random programming courses
            int courseCount = random.Next(10, 21);

            return Enumerable.Range(1, courseCount)
                .Select(i => new MyCourse
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"{ProgrammingLanguages[random.Next(ProgrammingLanguages.Length)]} {CourseTypes[random.Next(CourseTypes.Length)]} Course",
                    Language = ProgrammingLanguages[random.Next(ProgrammingLanguages.Length)],
                    CompletionPercentage = Math.Round(random.NextDouble(), 2), // Random completion between 0 and 1
                    EnrollmentDate = DateTime.Now.AddDays(-random.Next(1, 365)) // Enrolled within last year
                })
                .ToList();
        }

    }
}