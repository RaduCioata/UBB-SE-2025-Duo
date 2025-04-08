using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Duo.ViewModels;
using Duo.Models;
using Duo.Services;
using Duo;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DuolingoNou.Views.Pages
{
    public sealed partial class AchievementsPage : Page
    {
        private ProfileViewModel _viewModel;
        private ProfileService _profileService;

        public AchievementsPage()
        {
            this.InitializeComponent();
            _viewModel = App.ServiceProvider.GetRequiredService<ProfileViewModel>();
            _profileService = App.ServiceProvider.GetRequiredService<ProfileService>();
            
            LoadUserStats();
            LoadUserAchievements();
        }

        private void LoadUserStats()
        {
            User userStats = _viewModel.GetUserStats();

            if (userStats != null)
            {
                TotalXPText.Text = $"Total XP: {userStats.TotalPoints}";
                BestStreakText.Text = $"Best Streak: {userStats.Streak}";
                QuizzesCompletedText.Text = $"Quizzes Completed: {userStats.QuizzesCompleted}";
                CoursesCompletedText.Text = $"Courses Completed: {userStats.CoursesCompleted}";
            }
        }

        private void LoadUserAchievements()
        {
            User currentUser = _profileService.GetUserStats(App.CurrentUser.UserId);
            _profileService.AwardAchievements(currentUser);

            List<Achievement> userAchievements = _profileService.GetUserAchievements(App.CurrentUser.UserId);

            if (userAchievements != null && userAchievements.Count > 0)
            {
                AchievementsList.ItemsSource = userAchievements;
                System.Diagnostics.Debug.WriteLine("User achievements loaded");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No achievements found for user");
            }
        }
    }
}