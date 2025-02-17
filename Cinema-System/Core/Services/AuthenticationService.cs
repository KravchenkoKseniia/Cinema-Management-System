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
        
        public string Register(string userName, string password, string email, string role)
        {
            int roleId = role == "User" ? 2 : 1;
            var user = new User
            {
                UserName = userName,
                Email = email, 
                RoleId = roleId
            };

            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                // Automatically assign the "User" role
                var addRoleResult = _userManager.AddToRoleAsync(user, role).Result;
                if (!addRoleResult.Succeeded)
                {
                    return string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                }
                return "Registration successful!";
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
