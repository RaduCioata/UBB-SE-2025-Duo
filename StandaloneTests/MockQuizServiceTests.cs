using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StandaloneTests.Services;
using Xunit;

namespace StandaloneTests
{
    public class MockQuizServiceTests
    {
        private readonly MockQuizService _quizService;
        
        public MockQuizServiceTests()
        {
            _quizService = new MockQuizService();
        }
        
        [Fact]
        public async Task GetCompletedQuizzesAsync_ReturnsNonEmptyList()
        {
            // Act
            var quizzes = await _quizService.GetCompletedQuizzesAsync();
            
            // Assert
            Assert.NotNull(quizzes);
            Assert.NotEmpty(quizzes);
            Assert.InRange(quizzes.Count, 5, 15); // Based on constants in the service
        }
        
        [Fact]
        public async Task GetCompletedQuizzesAsync_ReturnsQuizzesWithValidProperties()
        {
            // Act
            var quizzes = await _quizService.GetCompletedQuizzesAsync();
            var firstQuiz = quizzes.FirstOrDefault();
            
            // Assert
            Assert.NotNull(firstQuiz);
            Assert.NotEmpty(firstQuiz.Id);
            Assert.NotEmpty(firstQuiz.Title);
            Assert.NotEmpty(firstQuiz.Language);
            Assert.InRange(firstQuiz.QuestionsCount, 5, 20);
            Assert.InRange(firstQuiz.CorrectAnswersCount, 0, firstQuiz.QuestionsCount);
            Assert.InRange(firstQuiz.Score, 0.0, 1.0);
            Assert.InRange(firstQuiz.CompletionDate, DateTime.Now.AddDays(-180), DateTime.Now);
        }
        
        [Fact]
        public async Task GetCompletedQuizzesAsync_GeneratesDifferentQuizzesEachTime()
        {
            // Act
            var firstResult = await _quizService.GetCompletedQuizzesAsync();
            var secondResult = await _quizService.GetCompletedQuizzesAsync();
            
            // We'll extract the IDs to compare the quiz sets
            var firstQuizIds = firstResult.Select(q => q.Id).ToList();
            var secondQuizIds = secondResult.Select(q => q.Id).ToList();
            
            // Assert
            // Verify either the count or contents are different (since this is random generation)
            bool hasDifferences = firstQuizIds.Count != secondQuizIds.Count ||
                                 !firstQuizIds.All(id => secondQuizIds.Contains(id));
            
            Assert.True(hasDifferences, "Mock quiz service should generate different data on each call");
        }
        
        [Fact]
        public async Task GetCompletedQuizzesAsync_FormattedScoreIsCorrect()
        {
            // Act
            var quizzes = await _quizService.GetCompletedQuizzesAsync();
            
            // Assert
            foreach (var quiz in quizzes)
            {
                string expected = $"{quiz.Score * 100:F0}%";
                Assert.Equal(expected, quiz.FormattedScore);
            }
        }
        
        [Fact]
        public async Task GetCompletedQuizzesAsync_ScoreMatchesCorrectAnswerRatio()
        {
            // Act
            var quizzes = await _quizService.GetCompletedQuizzesAsync();
            
            // Assert
            foreach (var quiz in quizzes)
            {
                // Allow for small floating point differences due to rounding
                double calculatedScore = Math.Round((double)quiz.CorrectAnswersCount / quiz.QuestionsCount, 2);
                Assert.Equal(calculatedScore, quiz.Score, 2); // 2 decimal places precision
            }
        }
    }
} 