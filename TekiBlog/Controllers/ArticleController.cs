using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TekiBlog.ViewModels;
using DataObjects;
using BusinessObjects;
using DataObjects.Repository;
using Microsoft.Extensions.Logging;
using ActionServices;
using ValidationUtilities;
using System.IO;

namespace TekiBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly IService _service;
        private readonly IGenericValidationUtil<CreateArticleViewModel> _validation;

        public ArticleController(UserManager<ApplicationUser> userManager,
            ILogger<ArticleController> logger,
            IService service,
            IGenericValidationUtil<CreateArticleViewModel> validation)
        {
            _userManager = userManager;
            _logger = logger;
            _service = service;
            _validation = validation;
        }

        public IActionResult Index()
        {
            // TODO: REMOVE LATER, FOR TEST ONLY

            List<Article> articles = _service.GetAllArticle().ToList();

            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == null)
            {
                _logger.LogInformation("article id is null");
                return NotFound();
            }

            var article = _service.GetArticle(id);
            if (article == null)
            {
                _logger.LogInformation($"request for article {id} returned null");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"status of article {article.ID}: {article.Status.Name}");
                var user = await _userManager.GetUserAsync(User);
                if (user == null || !article.User.Equals(user))
                {
                    _logger.LogInformation($"user {user} called for article {id}");
                    if (!article.Status.Name.Equals("Active"))
                    {
                        return NotFound();
                    }
                }
                else
                {
                    _logger.LogInformation("user is authorized");
                    if (article.Status.Name.Equals("Deleted"))
                    {
                        return NotFound();
                    }
                }
            }



            // Temporary block, will remove during deployment
            if (article.CoverImage != null)
            {
                string imageBase64Data = Convert.ToBase64String(article.CoverImage);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewData["ArticleCoverImg"] = imageDataURL;
            }

            //article.User = _context.Users.First(m => m.Id == article.User);
            _logger.LogInformation($"user receives article {article.ID}");

            return View(article);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Editor(Guid id)
        {
            #region setup validaiton viewdata
            ViewData["SummaryMaxLen"] = _validation.GetMaxLen("Summary");
            ViewData["SummaryMinLen"] = _validation.GetMinLen("Summary");

            ViewData["TitleMaxLen"] = _validation.GetMaxLen("Title");
            ViewData["TitleMinLen"] = _validation.GetMinLen("Title");

            ViewData["ContentMaxLen"] = _validation.GetMaxLen("ArticleRaw");
            ViewData["ContentMinLen"] = _validation.GetMinLen("ArticleRaw");
            #endregion

            if (id != null)
            {
                var article = _service.GetArticle(id);
                if (article != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (article.User.Id.Equals(user?.Id))
                    {
                        return View(new CreateArticleViewModel
                        {
                            ArticleContent = article.ContentHtml,
                            Title = article.Title,
                            Summary = article.Summary
                        });
                    }
                }
            }

            return View();
        }

        //[HttpPost]
        //public string AjaxTest(CreateArticleViewModel article)
        //{
        //    string received = article.ArticleContent;

        //    Console.Write(received);

        //    return received;
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostArticle(CreateArticleViewModel article)
        {
            // Get user in current context
            var user = await _userManager.GetUserAsync(User);

            _service.GetImage(out byte[] coverImage, this.Request);
            article.CoverImage = coverImage;

            if (ModelState.IsValid && coverImage != null)
            {

                // Get active status for this post
                Status active = _service.GetStatus("Active");

                string html = article.ArticleContent;

                string raw = article.ArticleRaw;

                // Create article model to insert to Database
                Article articleModel = new Article
                {
                    Title = article.Title?.Trim(),
                    Summary = article.Summary?.Trim(),
                    DatePosted = DateTime.UtcNow,
                    CurrentVote = 0,
                    ContentHtml = html,
                    ContentRaw = raw?.Trim(),
                    Status = active,
                    User = user,
                    CoverImage = article.CoverImage
                };

                _service.AddArticle(articleModel);
                if (await _service.Commit())
                {
                    _logger.LogInformation($"user {user.Id} posted article {articleModel.ID}");
                    return RedirectToAction("Detail", "Article", new { id = articleModel.ID });

                }
            }
            else
                _logger.LogInformation($"user {user.Id} post article failed");

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArticle(CreateArticleViewModel article)
        {
            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            _service.GetImage(out byte[] coverImage, this.Request);
            article.CoverImage = coverImage;

            if (ModelState.IsValid)
            {
                var pastArticle = _service.GetArticle(article.Id);
                if (pastArticle.User.Id.Equals(user.Id))
                {
                    // Get active status for this post
                    Status active = _service.GetStatus("Active");

                    // edit article model to update to Database
                    pastArticle.ContentHtml = article.ArticleContent;
                    pastArticle.ContentRaw = article.ArticleRaw?.Trim();
                    pastArticle.Title = article.Title?.Trim();
                    pastArticle.Summary = article.Summary?.Trim();
                    pastArticle.Status = active;

                    if (article.CoverImage != null)
                        pastArticle.CoverImage = article.CoverImage;

                    _service.UpdateArticle(pastArticle);
                    if (await _service.Commit())
                    {
                        _logger.LogInformation($"user {user.Id} updated article {pastArticle.ID}");
                        return RedirectToAction("Detail", "Article", new { id = pastArticle.ID });

                    } // exit if unable to update
                }
                else // exit if invalid user
                    _logger.LogInformation($"user {user.Id} tried updating {pastArticle.User.Id}'s article");
            } // exit if invalid article

            _logger.LogInformation($"user {user.Id} update article failed");
            return NotFound();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> ModifyArticle(Guid id, string type)
        {
            var article = _service.GetArticle(id);
            if (article != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    if (user.Equals(article.User))
                    {
                        Status disableStatus = _service.GetStatus(type);
                        article.Status = disableStatus;
                        _service.UpdateArticle(article);
                        if (await _service.Commit())
                        {
                            return RedirectToAction("Index", "Profile");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Auth");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchValue, int? pageNumber)
        {
            int pageSize = 2;
            _logger.LogInformation("Search value " + searchValue);
            if (string.IsNullOrEmpty(searchValue) && (pageNumber == null))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["SearchValue"] = searchValue;
            IQueryable<Article> articles = _service.SearchArticle(searchValue);
            PaginatedList<Article> result = await PaginatedList<Article>.CreateAsync(articles.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(result);
        }
    }
}
