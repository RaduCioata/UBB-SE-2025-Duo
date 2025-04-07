namespace Duo.Models;

public class LeaderboardEntry
{
    public int UserId { get; set; }
    public int Rank { get; set; }
    public string ProfilePicture { get; set; }
    public string Username { get; set; }
    public decimal Accuracy { get; set; }
    public int CompletedQuizzes { get; set; }

}
