﻿using System;
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

        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookmarkAdd(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            bool hasError;
            try
            {
                Bookmark bookmark = new Bookmark
                {
                    UserID = user.Id,
                    ArticleID = id,
                    DatePosted = DateTime.UtcNow
                };
    
                _service.AddBookmark(bookmark);
                hasError = !(await _service.Commit());
            }
            catch
            {
                _logger.LogInformation($"failed to book mark article {id}");
                hasError = true;
            }

            if(hasError)
                return Ok("error");
            else
                return Ok(0);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserBookmark()
        {
            var user = await _userManager.GetUserAsync(User);
            BookmarkViewModel viewModel = new BookmarkViewModel { Bookmarks = _service.GetBookmarks(user, true)};
            return View(viewModel);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookmarkRemove(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            bool hasError;
            try
            {
                Bookmark bookmark = new Bookmark
                {
                    UserID = user.Id,
                    ArticleID = id,
                    DatePosted = DateTime.UtcNow
                };

                _service.RemoveBookmark(bookmark);
                hasError = !( await _service.Commit() );
            }
            catch
            {
                _logger.LogInformation($"failed to remove bookmark of article {id}");
                hasError = true;
            }

            if (hasError)
                return Ok("error");
            else
                return Ok(1);
        }
    }
}
