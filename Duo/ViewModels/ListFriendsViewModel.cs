using Duo.Models;
using Duo.Services;
using Duo.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Duo;

namespace Duo.UI.ViewModels
{
    public class ListFriendsViewModel : INotifyPropertyChanged
    {
        private readonly FriendsService _friendsService;
        private List<User> _friends = new List<User>();
        private int _userId;

        // Implement the PropertyChanged event from INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Property for friends list
        public List<User> Friends
        {
            get => _friends;
            set
            {
                if (_friends != value)
                {
                    _friends = value;
                    // Raise the PropertyChanged event whenever the value of Friends changes
                    OnPropertyChanged(nameof(Friends));
                }
            }
        }

        public ListFriendsViewModel(FriendsService friendsService)
        {
            _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
            
            // User ID will be set when LoadFriends is called with the current user
            _userId = 0;
        }

        // Method to load friends from the service
        public void LoadFriends()
        {
            if (App.CurrentUser != null)
            {
                _userId = App.CurrentUser.UserId;
                var friends = _friendsService.GetFriends(_userId);
                Friends = friends;
            }
        }

        // Sort by name method
        public void SortByName()
        {
            var sortedFriends = _friendsService.SortFriendsByName(_userId);
            Friends = sortedFriends;  // Update the list
        }

        // Sort by date added method
        public void SortByDateAdded()
        {
            var sortedFriends = _friendsService.SortFriendsByDateAdded(_userId);
            Friends = sortedFriends;  // Update the list
        }

        public void SortByOnlineStatus()
        {
            var sortedFriends = _friendsService.SortFriendsByOnlineStatus(_userId);
            Friends = sortedFriends;  // Update the list
        }

        // Helper method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
