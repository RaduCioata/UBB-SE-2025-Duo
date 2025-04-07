using System;
using Duo.Services;
using Duo.Models;
using System.Threading.Tasks;
using Duo.Interfaces;

namespace Duo.ViewModels
{
    public class SignUpViewModel
    {
        private readonly SignUpService _signUpService;

        public SignUpViewModel(SignUpService signUpService)
        {
            _signUpService = signUpService ?? throw new ArgumentNullException(nameof(signUpService));
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _signUpService.IsUsernameTaken(username);
        }

        public async Task<bool> CreateNewUser(User user)
        {
            try
            {
                user.DateJoined = DateTime.Now;
                return await _signUpService.RegisterUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return false;
            }
        }
    }
}