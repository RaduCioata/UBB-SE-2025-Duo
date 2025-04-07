using Duo.Models;
using Duo.Repositories;
using Duo.Interfaces;
using System;
using System.Collections.Generic;

namespace Duo.Services;

public class LeaderboardService
{
    private readonly IUserRepository _userRepository;
    private readonly IFriendsRepository _friendsRepository;

    public LeaderboardService(IUserRepository userRepository, IFriendsRepository friendsRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _friendsRepository = friendsRepository ?? throw new ArgumentNullException(nameof(friendsRepository));
    }

    public List<LeaderboardEntry> GetGlobalLeaderboard(string criteria)
    {
        //return the first 10 users in the repo sorted by completed quizzes
        if (criteria == "CompletedQuizzes")
        {
            return _userRepository.GetTopUsersByCompletedQuizzes();
        }
        //return the first 10 users in the repo sorted by accurracy
        else if (criteria == "Accuracy")
        {
            return _userRepository.GetTopUsersByAccuracy();
        }
        else
        {
            throw new System.Exception("Invalid criteria");
        }
    }
    public List<LeaderboardEntry> GetFriendsLeaderboard(int userId, string criteria)
    {
        //return the first 10 users in the repo sorted by completed quizzes
        if (criteria == "CompletedQuizzes")
        {
            return _friendsRepository.GetTopFriendsByCompletedQuizzes(userId);
        }
        //return the first 10 users in the repo sorted by accurracy
        else if (criteria == "Accuracy")
        {
            return _friendsRepository.GetTopFriendsByAccuracy(userId);
        }
        else
        {
            throw new System.Exception("Invalid criteria");
        }
    }


    public void UpdateUserScore(int userId, int points)
    {
        //TODO: Implement this method
    }
    public void CalculateRankChange(int userId, string timeFrame)
    {
        //TODO: Implement this method
    }
}

