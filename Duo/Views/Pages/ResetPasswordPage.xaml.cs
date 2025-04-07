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
using Duo;
using Duo.Views.Pages;
using Duo.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DuolingoNou.Views.Pages
{
    /// <summary>
    /// Interaction logic for ResetPasswordPage.xaml
    /// </summary>
    public partial class ResetPasswordPage : Page
    {
        private readonly ResetPassViewModel resetPassViewModel;

        public ResetPasswordPage()
        {
            InitializeComponent();
            
            // Get the ResetPassViewModel from DI container
            resetPassViewModel = App.ServiceProvider.GetRequiredService<ResetPassViewModel>();
            DataContext = resetPassViewModel;
        }

        private async void OnSendCodeClick(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                StatusMessage.Text = "Please enter your email address.";
                return;
            }

            // Disable button while processing
            var button = sender as Button;
            if (button != null)
                button.IsEnabled = false;
                
            StatusMessage.Text = "Sending verification code...";

            bool isCodeSent = await resetPassViewModel.SendVerificationCode(email);

            if (isCodeSent)
            {
                StatusMessage.Text = "Verification code sent. Please check your email.";
                EmailPanel.Visibility = Visibility.Collapsed;
                CodePanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusMessage.Text = "Failed to send verification code. Please try again.";
                if (button != null)
                    button.IsEnabled = true;
            }
        }

        private void OnVerifyCodeClick(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text;

            if (string.IsNullOrWhiteSpace(code))
            {
                StatusMessage.Text = "Please enter the verification code.";
                return;
            }

            bool isVerified = resetPassViewModel.VerifyCode(code);

            if (isVerified)
            {
                StatusMessage.Text = "Code verified. Please enter your new password.";
                CodePanel.Visibility = Visibility.Collapsed;
                PasswordPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusMessage.Text = "Invalid verification code. Please try again.";
            }
        }

        private void OnResetPasswordClick(object sender, RoutedEventArgs e)
        {
            resetPassViewModel.NewPassword = NewPasswordBox.Password;
            resetPassViewModel.ConfirmPassword = ConfirmPasswordBox.Password;

            if (resetPassViewModel.NewPassword != resetPassViewModel.ConfirmPassword)
            {
                StatusMessage.Text = "Passwords don't match!";
                return;
            }

            bool isReset = resetPassViewModel.ResetPassword(resetPassViewModel.NewPassword);

            if (isReset)
            {
                StatusMessage.Text = "Password reset successfully!";
                
                // Navigate back to login page
                Frame.Navigate(typeof(LoginPage));
            }
            else
            {
                StatusMessage.Text = "Failed to reset password. Please try again.";
            }
        }
    }
}