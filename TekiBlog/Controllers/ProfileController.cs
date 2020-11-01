using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionServices;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(ILogger<ProfileController> logger,
            IService service,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _service = service;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogInformation("User is not here");
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                IQueryable<Article> articles = _service.GetArticleWithUserID(user);
                //_logger.LogInformation($"Article any {articles.Any()}");
                return View(articles);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var user = await _userManager.GetUserAsync(User);
            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                UserID = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
            return View(profileViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Info(ProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (viewModel.UserID.Equals(user.Id))
                {
                    if (user.FirstName.Equals(viewModel.FirstName) &&
                        user.LastName.Equals(viewModel.LastName) &&
                        user.Email.Equals(viewModel.Email) &&
                        user.UserName.Equals(viewModel.UserName))
                    {
                        return View();
                    }
                    user.FirstName = viewModel.FirstName;
                    user.Email = viewModel.Email;
                    user.LastName = viewModel.LastName;
                    user.UserName = viewModel.UserName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        ViewData["UpdateInfoMessageSuccess"] = "Update information successfully";
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogInformation("Error : " + error.Description);
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            _logger.LogInformation("Changed profile");
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var changePasswordresult = await _userManager.ChangePasswordAsync(user
                    , viewModel.OldPassword, viewModel.NewPassword);
                if (changePasswordresult.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    ViewData["UpdatePasswordMessageSuccess"] = "Update password successfully";
                }
                else
                {
                    foreach (var error in changePasswordresult.Errors)
                    {
                        _logger.LogInformation("Error : " + error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Id.Equals(id))
                {
                    return RedirectToAction("Index", "Profile");
                }
            }
            var requestedUser = await _userManager.FindByIdAsync(id);
            if (requestedUser == null)
            {
                return NotFound();
            }
            else
            {
                IQueryable<Article> articles = _service.GetArticleForViewer(requestedUser);
                return View(new Tuple<ApplicationUser, IQueryable<Article>>(requestedUser,articles));
            }
        }
    }
}
