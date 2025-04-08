using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StandaloneTests.Models;

namespace StandaloneTests.Services
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetCompletedQuizzesAsync();
    }

    public class Quiz
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public double Score { get; set; }
        public int QuestionsCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public DateTime CompletionDate { get; set; }

        public string FormattedScore => $"{Score * 100:F0}%";
    }

    public class MockQuizService : IQuizService
    {
        // Constants for mock data
        private const int MockAsyncOperationDelayMilliseconds = 500;
        private const int MinimumRandomQuizCount = 5;
        private const int MaximumRandomQuizCount = 15;
        private const int MinimumQuestionCount = 5;
        private const int MaximumQuestionCount = 20;
        private const int MaximumCompletionDaysInPast = 180;
        private const int AccuracyDecimalPlaces = 2;

        private static readonly string[] ProgrammingLanguages = new[]
        {
            "Python", "JavaScript", "Java", "C#", "C++",
            "Ruby", "Swift", "Kotlin", "Go", "Rust"
        };

        private static readonly string[] QuizTypes = new[]
        {
            "Fundamentals", "Advanced Concepts", "Design Patterns",
            "Best Practices", "Common Pitfalls", "Syntax Review",
            "Framework Overview", "Libraries", "Testing"
        };

        public async Task<List<Quiz>> GetCompletedQuizzesAsync()
        {
            await Task.Delay(MockAsyncOperationDelayMilliseconds);
            var randomGenerator = new Random();

            // Generate random programming quizzes
            int quizCount = randomGenerator.Next(
                MinimumRandomQuizCount, 
                MaximumRandomQuizCount
            );

            return Enumerable.Range(1, quizCount)
                .Select(_ => 
                {
                    string language = GetRandomElement(ProgrammingLanguages, randomGenerator);
                    string quizType = GetRandomElement(QuizTypes, randomGenerator);
                    int questionsCount = randomGenerator.Next(MinimumQuestionCount, MaximumQuestionCount);
                    int correctAnswers = randomGenerator.Next(0, questionsCount + 1); // +1 because NextInt is exclusive on upper bound

                    return new Quiz
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = $"{language} {quizType}",
                        Language = language,
                        QuestionsCount = questionsCount,
                        CorrectAnswersCount = correctAnswers,
                        Score = Math.Round((double)correctAnswers / questionsCount, AccuracyDecimalPlaces),
                        CompletionDate = DateTime.Now.AddDays(-randomGenerator.Next(1, MaximumCompletionDaysInPast))
                    };
                })
                .ToList();
        }

        private string GetRandomElement(string[] array, Random randomGenerator)
        {
            return array[randomGenerator.Next(array.Length)];
        }
    }
} 