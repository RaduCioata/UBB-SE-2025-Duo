namespace Duo.Models;

/// <summary>
/// Represents an entry in the leaderboard with user ranking information
/// </summary>
public class LeaderboardEntry
{
    /// <summary>
    /// Gets or sets the unique identifier of the user
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the user's position in the leaderboard
    /// </summary>
    public int Rank { get; set; }
    
    /// <summary>
    /// Gets or sets the path to the user's profile image
    /// </summary>
    public string ProfileImagePath { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display name of the user
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the score value that can represent either accuracy or another metric
    /// </summary>
    public decimal ScoreValue { get; set; }
}
