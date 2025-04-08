using System;

namespace SimpleServiceTests.Models
{
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
} 