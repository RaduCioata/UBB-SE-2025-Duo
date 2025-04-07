using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.Models;
using DuolingoNou.Models;

namespace DuolingoNou.Services
{

    public class MockQuizService : IQuizService
    {
        private static readonly string[] QuizTopics = new[]
        {
            "Syntax", "Data Structures", "Algorithms", "Object-Oriented Programming",
            "Functional Programming", "Memory Management", "Concurrency",
            "Design Patterns", "Web Frameworks", "Database Interactions",
            "Error Handling", "Testing", "Performance Optimization",
            "Security", "Asynchronous Programming"
        };

        private static readonly string[] ProgrammingLanguages = new[]
        {
            "Python", "JavaScript", "Java", "C#", "C++",
            "Ruby", "Swift", "Kotlin", "Go", "Rust",
            "TypeScript", "PHP", "Scala", "Dart", "R"
        };

        private static readonly string[] QuizTypes = new[]
        {
            "Basic", "Challenge", "Comprehensive", "Speed", "Diagnostic"
        };

        public async Task<List<Quiz>> GetCompletedQuizzesAsync()
        {
            await Task.Delay(100); // Simulate async operation
            var random = new Random();

            // Generate 10-20 random programming quizzes
            int quizCount = random.Next(10, 21);

            return Enumerable.Range(1, quizCount)
                .Select(i => new Quiz
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"{ProgrammingLanguages[random.Next(ProgrammingLanguages.Length)]} {QuizTypes[random.Next(QuizTypes.Length)]} {QuizTopics[random.Next(QuizTopics.Length)]} Quiz",
                    Category = QuizTopics[random.Next(QuizTopics.Length)],
                    AccuracyPercentage = Math.Round(random.NextDouble(), 2), // Random accuracy between 0 and 1
                    CompletionDate = DateTime.Now.AddDays(-random.Next(1, 365)) // Completed within last year
                })
                .ToList();
        }
    }

    public interface IQuizService
    {
        Task<List<Quiz>> GetCompletedQuizzesAsync();
    }

}