using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Views.Pages;
using Duo.ViewModels;
using Duo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Duo
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly ILoginService loginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            
            // Get the login service from the dependency injection container
            loginService = App.ServiceProvider.GetRequiredService<ILoginService>();
            
            // Navigate to the login page
            MainFrame.Navigate(typeof(LoginPage));
            
            // Handle the Closed event
            this.Closed += MainWindow_Closed;
        }

        /// <summary>
        /// Updates user status when the window is closed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            if (App.CurrentUser != null)
            {
                // Update the user's status to offline
                loginService.UpdateUserStatusOnLogout(App.CurrentUser);
                
                // Write diagnostic information
                System.Diagnostics.Debug.WriteLine($"{App.CurrentUser.UserName} (ID: {App.CurrentUser.UserId}) has logged out.");
            }
        }
    }
}