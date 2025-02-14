using Microsoft.AspNetCore.Identity;
using Infrastructure.Entities;
using Cinema_System.Services.Interfaces;

namespace Cinema_System.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        
        public string Register(string userName, string password, string role)
        {
            var user = new User
            {
                UserName = userName
            };

            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                var roleExists = _roleManager.RoleExistsAsync(role).Result;
                if (roleExists)
                {
                    _userManager.AddToRoleAsync(user, role).Wait();
                    return "Registration successful!";
                }
                return "Role does not exist.";
            }

            return string.Join(", ", result.Errors.Select(e => e.Description));
        }
        
        public string Login(string userName, string password)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            if (user == null)
            {
                return "User not found!";
            }

            var result = _signInManager.PasswordSignInAsync(user, password, false, false).Result; // Blocking call
            if (result.Succeeded)
            {
                return "Login successful!";
            }

            return "Invalid credentials!";
        }
        
        public bool IsAdmin(int userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return false;
            }

            var roles = _userManager.GetRolesAsync(user).Result;
            return roles.Contains("Admin");
        }
    }
}
