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
using Duo.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DuolingoNou.Views.Pages
{
    /// <summary>
    /// Main page of the application that displays user details and friends
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ProfileService _profileService;
        
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
            this._profileService = App.ServiceProvider.GetRequiredService<ProfileService>();
            this.ViewModel = App.ServiceProvider.GetRequiredService<ListFriendsViewModel>();
            
            this.LoadUserDetails();
            this.ViewModel.LoadFriends(); // Load friends now that we have the ViewModel
            this.DataContext = this.ViewModel; // Set DataContext for binding
        }

        /// <summary>
        /// Loads user details from the profile service
        /// </summary>
        private void LoadUserDetails()
        {
            if (App.CurrentUser != null)
            {
                var user = this._profileService.GetUserStats(App.CurrentUser.UserId);
                
                // Update UI with user stats
                this.UsernameText.Text = user.UserName;
                this.FriendCountText.Text = "24 friends"; // Placeholder or get from friends service
                this.DayStreakText.Text = user.Streak.ToString();
                this.TotalXPText.Text = user.TotalPoints.ToString();
                this.QuizzesCompletedText.Text = user.QuizzesCompleted.ToString();
                this.CoursesCompletedText.Text = user.CoursesCompleted.ToString();
                
                // Set profile image if available
                if (!string.IsNullOrEmpty(user.ProfileImage))
                {
                    try
                    {
                        // Check if it's a valid URI
                        if (Uri.TryCreate(user.ProfileImage, UriKind.Absolute, out Uri? imageUri))
                        {
                            this.ProfileImageBrush.ImageSource = new BitmapImage(imageUri);
                        }
                        else if (File.Exists(user.ProfileImage))
                        {
                            // Try as a local file path
                            this.ProfileImageBrush.ImageSource = new BitmapImage(new Uri(user.ProfileImage, UriKind.Absolute));
                        }
                    }
                    catch (Exception loadProfimeImageException)
                    {
                        // Log the exception but don't crash
                        System.Diagnostics.Debug.WriteLine($"Error loading profile image: {loadProfimeImageException.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Handles the selection change event of the sort combo box
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SortComboBox.SelectedIndex == 0)
            {
                this.ViewModel.SortByName();
            }
            else if (this.SortComboBox.SelectedIndex == 1)
            {
                this.ViewModel.SortByDateAdded();
            }
            else if (this.SortComboBox.SelectedIndex == 2)
            {
                this.ViewModel.SortByOnlineStatus();
            }
        }

        /// <summary>
        /// Handles the click event of the view all achievements button
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        private void OnViewAllClick(object sender, RoutedEventArgs e)
        {
            // Navigate to the achievements page
            this.Frame.Navigate(typeof(AchievementsPage));
        }

        /// <summary>
        /// Handles the click event of the profile image
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        private void OnProfileImageClick(object sender, TappedRoutedEventArgs e)
        {
            // Navigate to the profile settings page
            this.Frame.Navigate(typeof(ProfileSettingsPage));
        }

        /// <summary>
        /// Handles the click event of the update profile button
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The event data</param>
        private void OnUpdateProfileClick(object sender, RoutedEventArgs e)
        {
            // Navigate to the profile settings page
            this.Frame.Navigate(typeof(ProfileSettingsPage));
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
