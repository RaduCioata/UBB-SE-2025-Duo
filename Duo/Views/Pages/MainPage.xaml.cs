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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DuolingoNou.Views.Pages
{
    public sealed partial class MainPage : Page
    {
        private readonly ProfileService _profileService;
        public ListFriendsViewModel ViewModel { get; }

        public MainPage()
        {
            this.InitializeComponent();
            _profileService = App.ServiceProvider.GetRequiredService<ProfileService>();
            ViewModel = App.ServiceProvider.GetRequiredService<ListFriendsViewModel>();
            
            LoadUserDetails();
            ViewModel.LoadFriends(); // Load friends now that we have the ViewModel
            this.DataContext = ViewModel; // Set DataContext for binding
        }

        private void LoadUserDetails()
        {
            User currentUser = _profileService.GetUserStats(App.CurrentUser.UserId);

            if (currentUser != null)
            {
                UsernameText.Text = currentUser.UserName;
                FriendCountText.Text = $"18 friends";
                // ProfileImageBrush.ImageSource = new BitmapImage(new Uri(currentUser.ProfileImage));

                // Update statistics
                DayStreakText.Text = currentUser.Streak.ToString();
                TotalXPText.Text = currentUser.TotalPoints.ToString();
                QuizzesCompletedText.Text = currentUser.QuizzesCompleted.ToString();
                CoursesCompletedText.Text = currentUser.CoursesCompleted.ToString();

                // Award achievements
                _profileService.AwardAchievements(currentUser);
                System.Diagnostics.Debug.WriteLine("AwardAchievements called");
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedSortOption = selectedItem.Content.ToString();

                // Sort based on the selected option
                if (selectedSortOption == "Sort By Name")
                {
                    ViewModel.SortByName();
                }
                else if (selectedSortOption == "Sort By Date Added")
                {
                    ViewModel.SortByDateAdded();
                }
                else if (selectedSortOption == "Sort By Activity")
                {
                    ViewModel.SortByOnlineStatus();
                }
            }
        }

        private void OnProfileImageClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfileSettingsPage));
        }

        private void OnUpdateProfileClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfileSettingsPage));
        }

        private void OnViewAllClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AchievementsPage));
        }
    }
}
