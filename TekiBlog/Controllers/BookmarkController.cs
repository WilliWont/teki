using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionServices;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    public class BookmarkController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly IService _service;

        public BookmarkController(UserManager<ApplicationUser> userManager,
            ILogger<ArticleController> logger,
            IService service)
        {
            _userManager = userManager;
            _logger = logger;
            _service = service;
        }

        public IActionResult BookmarkAdd(Guid id)
        {
            return Ok(id);
        }

        public IActionResult BookmarkListDetail()
        {
            return View();
        }

        public IActionResult BookmarkRemove()
        {
            return View();
        }
    }
}
