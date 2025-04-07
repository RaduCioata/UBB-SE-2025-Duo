using Duo.Services;
using Duo;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Duo.Models;
using Duo.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Duo.Constants;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DuolingoNou.Views.Pages
{
    /// <summary>
    /// Main page of the application that displays user details and friends
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ProfileService profileService;
        
        /// <summary>
        /// Gets the view model for the friends list
        /// </summary>
        public ListFriendsViewModel ViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            profileService = App.ServiceProvider.GetRequiredService<ProfileService>();
            ViewModel = App.ServiceProvider.GetRequiredService<ListFriendsViewModel>();
            
            LoadUserDetails();
            ViewModel.LoadFriends(); // Load friends now that we have the ViewModel
            this.DataContext = ViewModel; // Set DataContext for binding
        }

        /// <summary>
        /// Loads the current user's details and updates the UI
        /// </summary>
        private void LoadUserDetails()
        {
            User currentUser = profileService.GetUserStats(App.CurrentUser.UserId);

            if (currentUser != null)
            {
                UsernameText.Text = currentUser.UserName;
                FriendCountText.Text = string.Format(UserInterfaceConstants.FriendCountTemplate, 18);
                // ProfileImageBrush.ImageSource = new BitmapImage(new Uri(currentUser.ProfileImage));

                // Update statistics
                DayStreakText.Text = currentUser.Streak.ToString();
                TotalXPText.Text = currentUser.TotalPoints.ToString();
                QuizzesCompletedText.Text = currentUser.QuizzesCompleted.ToString();
                CoursesCompletedText.Text = currentUser.CoursesCompleted.ToString();

                // Award achievements
                profileService.AwardAchievements(currentUser);
                System.Diagnostics.Debug.WriteLine("AwardAchievements called");
            }
        }

        /// <summary>
        /// Event handler for the name sort button click
        /// </summary>
        private void NameSortButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SortByName();
        }

        /// <summary>
        /// Event handler for the date added sort button click
        /// </summary>
        private void DateAddedSortButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SortByDateAdded();
        }

        /// <summary>
        /// Event handler for the online status sort button click
        /// </summary>
        private void OnlineStatusSortButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SortByOnlineStatus();
        }

        /// <summary>
        /// Event handler for the profile settings button click
        /// </summary>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfileSettingsPage));
        }

        /// <summary>
        /// Event handler for the achievements button click
        /// </summary>
        private void AchievementsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AchievementsPage));
        }
    }
}
