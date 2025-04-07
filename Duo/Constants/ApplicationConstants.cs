using System;

namespace Duo.Constants
{
    /// <summary>
    /// Application-wide constants for verification and authentication
    /// </summary>
    public static class VerificationConstants
    {
        /// <summary>
        /// Minimum value for a verification code
        /// </summary>
        public const int MinimumVerificationCodeValue = 100000;

        /// <summary>
        /// Maximum value for a verification code
        /// </summary>
        public const int MaximumVerificationCodeValue = 999999;

        /// <summary>
        /// Number of milliseconds to simulate API delay for verification code sending
        /// </summary>
        public const int VerificationCodeSendingDelayMilliseconds = 1000;
    }

    /// <summary>
    /// Application-wide constants for mock data generation
    /// </summary>
    public static class MockDataConstants
    {
        /// <summary>
        /// Milliseconds delay for simulating asynchronous operations in mock services
        /// </summary>
        public const int MockAsyncOperationDelayMilliseconds = 100;

        /// <summary>
        /// Minimum number of random quizzes to generate
        /// </summary>
        public const int MinimumRandomQuizCount = 10;

        /// <summary>
        /// Maximum number of random quizzes to generate
        /// </summary>
        public const int MaximumRandomQuizCount = 21;

        /// <summary>
        /// Maximum days in the past for generated completion dates
        /// </summary>
        public const int MaximumCompletionDaysInPast = 365;

        /// <summary>
        /// Minimum random quiz index
        /// </summary>
        public const int MinimumRandomIndex = 1;

        /// <summary>
        /// Decimal places for rounding accuracy percentages
        /// </summary>
        public const int AccuracyDecimalPlaces = 2;
    }

    /// <summary>
    /// Application-wide constants for leaderboard operations
    /// </summary>
    public static class LeaderboardConstants
    {
        /// <summary>
        /// Criteria for sorting by completed quizzes
        /// </summary>
        public const string CompletedQuizzesCriteria = "CompletedQuizzes";

        /// <summary>
        /// Criteria for sorting by accuracy
        /// </summary>
        public const string AccuracyCriteria = "Accuracy";

        /// <summary>
        /// Default value for no rank
        /// </summary>
        public const int NoRankValue = -1;

        /// <summary>
        /// Adjustment to convert from zero-based index to one-based rank
        /// </summary>
        public const int RankIndexAdjustment = 1;
    }

    /// <summary>
    /// Application-wide constants for UI-related values
    /// </summary>
    public static class UserInterfaceConstants
    {
        /// <summary>
        /// Default friend count message template
        /// </summary>
        public const string FriendCountTemplate = "{0} friends";
    }
} 