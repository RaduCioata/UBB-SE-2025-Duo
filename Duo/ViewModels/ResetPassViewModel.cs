using Duo.Interfaces;
using Duo.Repositories;
using Duo.Services;
using DuolingoNou.Services;
using System;
using System.Threading.Tasks;

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel for password reset functionality.
    /// </summary>
    public class ResetPassViewModel
    {
        private readonly ForgotPassService forgotPassService;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the verification code.
        /// </summary>
        public string VerificationCode { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets a value indicating whether the code is verified.
        /// </summary>
        public bool IsCodeVerified { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPassViewModel"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public ResetPassViewModel(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }
            
            forgotPassService = new ForgotPassService(userRepository);
        }

        /// <summary>
        /// Sends a verification code to the specified email.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>True if the code was sent successfully; otherwise, false.</returns>
        public async Task<bool> SendVerificationCode(string email)
        {
            Email = email;
            return await forgotPassService.SendVerificationCode(email);
        }

        /// <summary>
        /// Verifies the specified code.
        /// </summary>
        /// <param name="code">The verification code.</param>
        /// <returns>True if the code is valid; otherwise, false.</returns>
        public bool VerifyCode(string code)
        {
            IsCodeVerified = forgotPassService.VerifyCode(code);
            return IsCodeVerified;
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <returns>True if the password was reset successfully; otherwise, false.</returns>
        public bool ResetPassword(string newPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                StatusMessage = "Passwords don't match!";
                return false;
            }

            return forgotPassService.ResetPassword(Email, newPassword);
        }
    }
}