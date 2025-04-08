using System;

namespace Duo.Models;

public class User
{
    // User identification
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    
    // User privacy and activity settings
    public bool IsProfilePrivate { get; set; } = false;
    public bool IsCurrentlyOnline { get; set; } = false;
    public DateTime AccountCreationDate { get; set; } = DateTime.Now;
    public string ProfileImagePath { get; set; } = "default.jpg";
    
    // User statistics
    public int TotalPointsEarned { get; set; } = 0;
    public int CompletedCoursesCount { get; set; } = 0;
    public int CompletedQuizzesCount { get; set; } = 0;
    public int ConsecutiveDaysStreak { get; set; } = 0;
    
    // Authentication
    public string Password { get; set; } = string.Empty;

    // User activity tracking
    public DateTime? LastUserActivityTimestamp { get; set; }
    public decimal AnswerAccuracyPercentage { get; set; } = 0.00m;

    public string OnlineStatusDisplayText => IsCurrentlyOnline ? "Active" : "Not Active";

    public string GetLastSeenDisplayText
    {
        get
        {
            if (IsCurrentlyOnline)
            {
                return "Active Now";
            }

            if (LastUserActivityTimestamp.HasValue)
            {
                var timeElapsedSinceLastActivity = DateTime.Now - LastUserActivityTimestamp.Value;

                if (timeElapsedSinceLastActivity.TotalMinutes < 1)
                {
                    return "Less than a minute ago";
                }
                else if (timeElapsedSinceLastActivity.TotalHours < 1)
                {
                    return $"{Math.Floor(timeElapsedSinceLastActivity.TotalMinutes)} minutes ago";
                }
                else if (timeElapsedSinceLastActivity.TotalDays < 1)
                {
                    return $"{Math.Floor(timeElapsedSinceLastActivity.TotalHours)} hours ago";
                }
                else
                {
                    return $"{Math.Floor(timeElapsedSinceLastActivity.TotalDays)} days ago";
                }
            }

            return "Last seen a long time ago";
        }
    }
}
