using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionServices;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IService _service;

        public AdminController(UserManager<ApplicationUser> userManager,
            ILogger<AdminController> logger,
            IService service)
        {
            _userManager = userManager;
            _logger = logger;
            _service = service;
        }
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = PaginatedList<Article>.PerPage;
            IQueryable<Article> articles = _service.GetArticleForAdmin();
            PaginatedList<Article> result = await PaginatedList<Article>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(result);
        }

        public async Task<IActionResult> ViewUser(int? pageNumber)
        {
            int pageSize = PaginatedList<ApplicationUser>.PerPage;
            var users = _userManager.Users.AsQueryable();
            PaginatedList<ApplicationUser> result = await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(result);
        }
        [HttpGet]
        public IActionResult Tag()
        {
            var tags = _service.GetAllTags();
            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return RedirectToAction("Tag", "Admin");
            }
            Tag newTag = new Tag
            {
                Name = tagName
            };
            _service.CreateTag(newTag);
            if (await _service.Commit())
            {
                _logger.LogInformation("Create tag successfully");
                TempData["CreateMessageStatus"] = "Create tag successfully";
            }
            else
            {
                _logger.LogInformation("Create tag unsuccessfully");
                TempData["CreateMessageStatus"] = " Create tag unsuccessfully";
            }
            return RedirectToAction("Tag", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTag(int id)
        {
            bool deleteStatus = _service.DeleteTag(id);
            if (!deleteStatus)
            {
                _logger.LogInformation("Invalid ID to delete");
                TempData["DeleteMessageStatus"] = "Invalid ID to delete";
            }
            else
            {
                if (await _service.Commit())
                {
                    _logger.LogInformation("Delete tag successfully");
                    TempData["DeleteMessageStatus"] = "Delete tag successfully";
                }
                else
                {
                    _logger.LogInformation("Delete tag unsuccessfully");
                    TempData["DeleteMessageStatus"] = " Delete tag unsuccessfully";
                }
            }
            return RedirectToAction("Tag", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteArticle(Guid id, int? pageNumber )
        {
            Article article = _service.GetArticle(id);
            if (article != null)
            {
                Status deleteStatus = _service.GetStatus("Deleted");
                article.Status = deleteStatus;
                _service.UpdateArticle(article);
                if (await _service.Commit())
                {
                    TempData["DeleteMessageStatus"] = "Delete Article successfully";
                }
                else
                {
                    TempData["DeleteMessageStatus"] = "Delete Article unsuccessfully";
                }
            }
            return RedirectToAction("Index", "Admin", pageNumber);
        }
        //[HttpGet]
        //public async Task<IActionResult> RestoreArticle(Guid id, int? pageNumber)
        //{
        //    Article article = _service.GetArticle(id);
        //    if (article != null)
        //    {
        //        Status deleteStatus = _service.GetStatus("Disable");
        //        article.Status = deleteStatus;
        //        _service.UpdateArticle(article);
        //        if (await _service.Commit())
        //        {
        //            TempData["RestoreMessageStatus"] = "Restore Article successfully";
        //        }
        //        else
        //        {
        //            TempData["RestoreMessageStatus"] = "Restore Article unsuccessfully";
        //        }
        //    }
        //    return RedirectToAction("Index", "Admin", pageNumber);
        //}
    }
}
