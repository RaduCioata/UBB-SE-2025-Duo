using Duo.Models;
using Duo.Services;
using System;

namespace Duo.ViewModels
{
    public class ProfileViewModel
    {
        private readonly ProfileService _profileService;

        public User CurrentUser { get; set; }

        public ProfileViewModel(ProfileService profileService, User user)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            CurrentUser = user ?? throw new ArgumentNullException(nameof(user));
        }

        public ProfileViewModel(ProfileService profileService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            
            // This will be null until App.CurrentUser is set
            CurrentUser = App.CurrentUser;
        }

        public void SaveChanges(bool isPrivate, string newBase64Image)
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set");
            }

            // Only update if a new image is provided
            if (!string.IsNullOrWhiteSpace(newBase64Image))
            {
                CurrentUser.ProfileImage = newBase64Image;
            }

            CurrentUser.PrivacyStatus = isPrivate;

            _profileService.UpdateUser(CurrentUser);
        }

        public User GetUserStats()
        {
            if (CurrentUser == null)
            {
                throw new InvalidOperationException("CurrentUser is not set");
            }
            
            return _profileService.GetUserStats(CurrentUser.UserId);
        }
    }
}
