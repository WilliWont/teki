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

namespace TekiBlog.Controllers
{
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ILogger<ProfileController> logger,
            IService service,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _service = service;
            _userManager = userManager;
        }
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
        public async Task<IActionResult> Info()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
    }
}
