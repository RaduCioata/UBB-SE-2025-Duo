using System;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duo.Data;
using Duo.Repositories;
using System.IO;
using DuolingoNou;
using DuolingoNou.Views;
using Duo.Models;
using Duo.Interfaces;
using Duo.Services;
using Duo.ViewModels;
using Duo.UI.ViewModels;

namespace Duo
{
    /// <summary>
    /// Main application class.
    /// </summary>
    public partial class App : Application
    {
        private static IConfiguration configuration;
        private static IServiceProvider serviceProvider;
        
        /// <summary>
        /// Gets the current user after login.
        /// </summary>
        public static User CurrentUser { get; set; }
        
        /// <summary>
        /// Gets the main application window.
        /// </summary>
        public static Window MainAppWindow { get; private set; }

        /// <summary>
        /// Gets the service provider for dependency injection.
        /// </summary>
        public static IServiceProvider ServiceProvider => serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            
            // Initialize configuration
            configuration = InitializeConfiguration();
            
            // Set up dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register configuration
            services.AddSingleton(configuration);
            
            // Register data access
            services.AddSingleton<IDataLink, DataLink>();
            
            // Register repositories
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IFriendsRepository, FriendsRepository>();
            
            // Register services
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<FriendsService>();
            services.AddTransient<SignUpService>();
            services.AddTransient<ProfileService>();
            services.AddTransient<LeaderboardService>();
            
            // Register view models
            services.AddTransient<LoginViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<ResetPassViewModel>();
            services.AddTransient<ListFriendsViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<LeaderboardViewModel>();
        }

        private IConfiguration InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// Handles the application launch.
        /// </summary>
        /// <param name="args">Launch arguments.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainAppWindow = new MainWindow();
            MainAppWindow.Activate();
        }

        private Window window;
    }
}