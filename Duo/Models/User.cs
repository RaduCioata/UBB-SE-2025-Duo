using System;

namespace Duo.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool PrivacyStatus { get; set; } = false; //0 = Public, 1 = Private
    public bool OnlineStatus { get; set; } = false; //0 = Offline, 1 = Online
    public DateTime DateJoined { get; set; } = DateTime.Now;//data la care a fost creat contul?
    public string ProfileImage { get; set; } = "default.jpg";
    public int TotalPoints { get; set; } = 0;
    public int CoursesCompleted { get; set; } = 0;
    public int QuizzesCompleted { get; set; } = 0;
    public int Streak { get; set; } = 0;
    public string Password { get; set; } = string.Empty;

    public DateTime? LastActivityDate { get; set; }
    public decimal Accuracy { get; set; } = 0.00m;

    public string OnlineStatusText => OnlineStatus == true ? "Active" : "Not Active";

    public string GetLastSeenText
    {
        get
        {
            if (OnlineStatus == true)
            {
                return "Active Now";
            }

            if (LastActivityDate.HasValue)
            {
                var timeAgo = DateTime.Now - LastActivityDate.Value;

                if (timeAgo.TotalMinutes < 1)
                {
                    return "Less than a minute ago";
                }
                else if (timeAgo.TotalHours < 1)
                {
                    return $"{Math.Floor(timeAgo.TotalMinutes)} minutes ago";
                }
                else if (timeAgo.TotalDays < 1)
                {
                    return $"{Math.Floor(timeAgo.TotalHours)} hours ago";
                }
                else
                {
                    return $"{Math.Floor(timeAgo.TotalDays)} days ago";
                }
            }

            return "Last seen a long time ago"; // Or any other fallback message
        }
    }

}
