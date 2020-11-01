using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TekiBlog.ViewModels;
using BusinessObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TekiBlog.Controllers
{
    public class AuthController : Controller
    {
        // Dependency Inject to SiginManager , UserManager and ILogger
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginViewModel> _logger;
        public AuthController(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager,
             ILogger<LoginViewModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Find userName by Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            string userName = "";
            if (user != null)
            {
                userName = user.UserName;
            }
            // Sign in with username and password
            var result = await _signInManager.PasswordSignInAsync(userName, model.Password, false, false);
            if (result.Succeeded)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                _logger.LogInformation("User Logged in");
                if (isAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                _logger.LogInformation("User Logged in failed");
                ViewData["ErrorLogin"] = "The password or email is invalid";
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // If all validations OK
            {
                MailAddress address = new MailAddress(model.Email);
                string username = address.User;
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                };
                // Create user by inject userManager
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) // If create new User successfully
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Add the dafault role : User
                    await _userManager.AddToRoleAsync(user, "User");
                    // Sign In to this user 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else // If create user unsuccessfully
                {
                    _logger.LogInformation("User create unsuccessfully");
                    // Get all errors
                    foreach (var error in result.Errors)
                    {
                        _logger.LogInformation("Error : " + error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage));
                var errors = ModelState.Select(v => new { key = v.Key, value = v.Value });
                foreach (var error in errors)
                {
                    _logger.LogInformation($"Error : {error.key} - {error.value} ");
                }
                _logger.LogError(message);
            }
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
