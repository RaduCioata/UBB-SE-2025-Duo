using Duo.Models;
using Duo.Repositories;
using Duo.Interfaces;
using DuolingoNou.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Duo.Services
{
    public class ProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void CreateUser(User user)
        {
            _userRepository.CreateUser(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public User GetUserStats(int userId)
        {
            return _userRepository.GetUserStats(userId);
        }

        public void AwardAchievements(User user)
        {
            List<Achievement> achievements = _userRepository.GetAllAchievements();
            List<Achievement> userAchievements = _userRepository.GetUserAchievements(user.UserId);

            foreach (var achievement in achievements)
            {
                if (!userAchievements.Any(a => a.Id == achievement.Id))
                {
                    if (achievement.Name.Contains("Streak") && user.Streak >= GetAchievementThreshold(achievement.Name))
                    {
                        _userRepository.AwardAchievement(user.UserId, achievement.Id);
                        System.Diagnostics.Debug.WriteLine($"Awarded achievement: {achievement.Name}");
                    }
                    else if (achievement.Name.Contains("Quizzes Completed") && user.QuizzesCompleted >= GetAchievementThreshold(achievement.Name))
                    {
                        _userRepository.AwardAchievement(user.UserId, achievement.Id);
                        System.Diagnostics.Debug.WriteLine($"Awarded achievement: {achievement.Name}");
                    }
                    else if (achievement.Name.Contains("Courses Completed") && user.CoursesCompleted >= GetAchievementThreshold(achievement.Name))
                    {
                        _userRepository.AwardAchievement(user.UserId, achievement.Id);
                        System.Diagnostics.Debug.WriteLine($"Awarded achievement: {achievement.Name}");
                    }
                }
            }
        }

        private int GetAchievementThreshold(string achievementName)
        {
            if (achievementName.Contains("10")) return 10;
            if (achievementName.Contains("50")) return 50;
            if (achievementName.Contains("100")) return 100;
            if (achievementName.Contains("250")) return 250;
            if (achievementName.Contains("500")) return 500;
            if (achievementName.Contains("1000")) return 1000;
            return 0;
        }

        public List<Achievement> GetUserAchievements(int userId)
        {
            return _userRepository.GetUserAchievements(userId);
        }
    }
}
