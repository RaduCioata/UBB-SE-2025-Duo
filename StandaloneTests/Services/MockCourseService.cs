using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StandaloneTests.Models;

namespace StandaloneTests.Services
{
    public interface ICourseService
    {
        Task<List<MyCourse>> GetEnrolledCoursesAsync();
    }

    public class MockCourseService : ICourseService
    {
        // Constants for mock data
        private const int MockAsyncOperationDelayMilliseconds = 500;
        private const int MinimumRandomQuizCount = 3;
        private const int MaximumRandomQuizCount = 8;
        private const int MinimumRandomIndex = 1;
        private const int AccuracyDecimalPlaces = 2;
        private const int MaximumCompletionDaysInPast = 365;

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
            await Task.Delay(MockAsyncOperationDelayMilliseconds);
            var randomGenerator = new Random();

            // Generate random programming courses
            int courseCount = randomGenerator.Next(
                MinimumRandomQuizCount, 
                MaximumRandomQuizCount
            );

            return Enumerable.Range(MinimumRandomIndex, courseCount)
                .Select(courseIndex => new MyCourse
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"{GetRandomElement(ProgrammingLanguages, randomGenerator)} {GetRandomElement(CourseTypes, randomGenerator)} Course",
                    Language = GetRandomElement(ProgrammingLanguages, randomGenerator),
                    CompletionPercentage = Math.Round(randomGenerator.NextDouble(), AccuracyDecimalPlaces),
                    EnrollmentDate = DateTime.Now.AddDays(-randomGenerator.Next(1, MaximumCompletionDaysInPast))
                })
                .ToList();
        }

        private string GetRandomElement(string[] array, Random randomGenerator)
        {
            return array[randomGenerator.Next(array.Length)];
        }
    }
} 