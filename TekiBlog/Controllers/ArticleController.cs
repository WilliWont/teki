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
using System.Drawing;
using System.Drawing.Imaging;

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

        public async Task<IActionResult> Home(int? pageNumber, int? tagid)
        {
            _logger.LogInformation("Article home is called");
            IQueryable<Article> articles = null;
            if (tagid != null || tagid >= 1)
            {
                articles = _service.GetArticleByTag(tagid ?? 1);
            }
            else
            {
                articles = _service.GetArticleByStatus("Active");
            }

            foreach (var a in articles)
            {
                _logger.LogInformation($"Article {a.ID} : {a.ArticleTags.Count}");
                foreach (var at in a.ArticleTags)
                {
                    _logger.LogInformation($"Tag {at.TagId}");
                    _logger.LogInformation($"Tag {at.Tag.Name}");
                }
            }
            _logger.LogInformation("--------------------------------");
            int pageSize = PaginatedList<Article>.PerPage;

            PaginatedList<Article> result = await PaginatedList<Article>.CreateAsync(articles, pageNumber ?? 1, pageSize);

            //_logger.LogInformation("result size : " + result.Count());
            //foreach (var a in result)
            //{
            //    _logger.LogInformation($"Article {a.ID} : {a.ArticleTags.Count}");
            //    foreach (var at in a.ArticleTags)
            //    {
            //        _logger.LogInformation($"Tag {at.TagId}");
            //        _logger.LogInformation($"Tag {at.Tag}");
            //    }
            //}
            HomePageViewModel viewModel = new HomePageViewModel
            {
                Articles = result,
            };

            var user = await _userManager.GetUserAsync(User);
            if (user?.Id != null)
            {
                List<Bookmark> bookmarks = await _service.GetBookmarks(user).ToListAsync();
                viewModel.UserBookmarks = bookmarks;
            }


            return View(viewModel);

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
                        if (article.CoverImage != null)
                        {
                            string imageBase64Data = Convert.ToBase64String(article.CoverImage);
                            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                            ViewData["ArticleCoverImg"] = imageDataURL;
                        }

                        return View(new CreateArticleViewModel
                        {
                            ArticleContent = article.ContentHtml,
                            Title = article.Title,
                            Summary = article.Summary,
                            Status = article.Status,
                            ArticleTags = article.ArticleTags
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
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostArticle(CreateArticleViewModel article)
        {
            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            byte[] articleImage;

            try
            {
                _service.GetImage(out articleImage, this.Request);
                // TODO: elimate magic number if have time
                articleImage = _service.ResizeImgageByWidth(articleImage, 1280, ImageFormat.Jpeg);

                article.CoverImage = _service.CropImage(articleImage, new Rectangle(0, 0, 1280, 720), ImageFormat.Jpeg);
                article.ThumbnailImage = _service.CropImage(articleImage, new Rectangle(600 / 3, 0, 600, 450), ImageFormat.Jpeg);
            }
            catch
            {
                _logger.LogInformation("failed to retrieve image");
                articleImage = null;
            }

            string statusType = (ModelState.IsValid) ? "Active" : "Draft";

            // Get active status for this post
            Status status = _service.GetStatus(statusType);

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
                Status = status,
                User = user,
                CoverImage = article?.CoverImage,
                ThumbnailImage = article?.ThumbnailImage
            };

            _service.AddArticle(articleModel);

            if (await _service.Commit())
            {
                _logger.LogInformation($"user {user.Id} posted article {articleModel.ID} with status ${status.Name}");

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Detail", "Article", new { id = articleModel.ID });
                }
                else
                {
                    return RedirectToAction("Editor", "Article", new { id = articleModel.ID });
                }

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArticle(CreateArticleViewModel article)
        {
            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            byte[] articleImage;
            try
            {
                _service.GetImage(out articleImage, this.Request);
                // TODO: elimate magic number if have time
                articleImage = _service.ResizeImgageByWidth(articleImage, 1280, ImageFormat.Jpeg);

                article.CoverImage = _service.CropImage(articleImage, new Rectangle(0, 0, 1280, 720), ImageFormat.Jpeg);
                article.ThumbnailImage = _service.CropImage(articleImage, new Rectangle(600 / 3, 0, 600, 450), ImageFormat.Jpeg);
            }
            catch
            {
                _logger.LogInformation("failed to retrieve image");
                articleImage = null;
            }
            var pastArticle = _service.GetArticle(article.Id);
            if (pastArticle.User.Id.Equals(user.Id))
            {
                string statusType = (ModelState.IsValid) ? "Active" : "Draft";

                // Get active status for this post
                Status status = _service.GetStatus(statusType);

                // edit article model to update to Database
                pastArticle.ContentHtml = article.ArticleContent;
                pastArticle.ContentRaw = article.ArticleRaw?.Trim();
                pastArticle.Title = article.Title?.Trim();
                pastArticle.Summary = article.Summary?.Trim();
                pastArticle.Status = status;

                if (ModelState.IsValid && pastArticle.DatePosted == DateTime.MinValue)
                {
                    pastArticle.DatePosted = DateTime.UtcNow;
                }

                if (articleImage != null)
                {
                    pastArticle.CoverImage = article.CoverImage;
                    pastArticle.ThumbnailImage = article.ThumbnailImage;
                }

                _service.UpdateArticle(pastArticle);

                if (await _service.Commit())
                {
                    _logger.LogInformation($"user {user.Id} updated article {pastArticle.ID} with status ${status.Name}");

                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Detail", "Article", new { id = pastArticle.ID });
                    }
                    else
                    {
                        return RedirectToAction("Editor", "Article", new { id = pastArticle.ID });
                    }
                }

            } // exit if invalid user
            else
                _logger.LogInformation($"user {user?.Id} tried updating {pastArticle.User.Id}'s article");

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ArticleDraft(CreateArticleViewModel article)
        {
            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            byte[] articleImage;
            try
            {
                _service.GetImage(out articleImage, this.Request);
                // TODO: elimate magic number if have time
                articleImage = _service.ResizeImgageByWidth(articleImage, 1280, ImageFormat.Jpeg);

                article.CoverImage = _service.CropImage(articleImage, new Rectangle(0, 0, 1280, 720), ImageFormat.Jpeg);
                article.ThumbnailImage = _service.CropImage(articleImage, new Rectangle(600 / 3, 0, 600, 450), ImageFormat.Jpeg);
            }
            catch
            {
                _logger.LogInformation("failed to retrieve image");
                articleImage = null;
            }

            var pastArticle = _service.GetArticle(article.Id);
            bool toUpdate = ((pastArticle != null) && pastArticle.User.Id.Equals(user.Id));

            Status draft = _service.GetStatus("Draft");

            if (toUpdate)
            {
                // Get active status for this post

                // edit article model to update to Database
                pastArticle.ContentHtml = article.ArticleContent;
                pastArticle.ContentRaw = article.ArticleRaw?.Trim();
                pastArticle.Title = article.Title?.Trim();
                pastArticle.Summary = article.Summary?.Trim();
                pastArticle.Status = draft;

                if (articleImage != null)
                {
                    pastArticle.CoverImage = article.CoverImage;
                    pastArticle.ThumbnailImage = article.ThumbnailImage;
                }

                _service.UpdateArticle(pastArticle);
            }
            else
            {
                Article toAdd = new Article
                {
                    ContentHtml = article.ArticleContent,
                    ContentRaw = article.ArticleRaw?.Trim(),
                    Title = article.Title?.Trim(),
                    Summary = article.Summary?.Trim(),
                    User = user,
                    DatePosted = DateTime.UtcNow,
                    Status = draft
                };


                if (articleImage != null)
                {
                    toAdd.CoverImage = article.CoverImage;
                    toAdd.ThumbnailImage = article.ThumbnailImage;
                }

                _service.AddArticle(toAdd);
            }


            if (await _service.Commit())
            {
                _logger.LogInformation($"user {user.Id} saved existing article {pastArticle?.ID}");
                return Ok();

            } // exit if unable to update

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
        public IActionResult Search(string searchValue, int? pageNumber)
        {
            int pageSize = PaginatedList<Article>.PerPage;
            _logger.LogInformation("Search value " + searchValue);
            if (string.IsNullOrEmpty(searchValue) && (pageNumber == null))
            {
                return RedirectToAction("Home", "Article");
            }
            ViewData["SearchValue"] = searchValue;
            IQueryable<Article> articles = _service.SearchArticle(searchValue);
            foreach (var a in articles)
            {
                _logger.LogInformation($"Article {a.ID} : {a.ArticleTags.Count}");
                foreach (var at in a.ArticleTags)
                {
                    _logger.LogInformation($"Tag {at.TagId}");
                    _logger.LogInformation($"Tag {at.Tag.Name}");
                }
            }
            PaginatedList<Article> result = PaginatedList<Article>.Create(articles, pageNumber ?? 1, pageSize);
            //_logger.LogInformation("result size : " + result.Count());
            //foreach (var a in result)
            //{
            //    _logger.LogInformation($"Article {a.ID} : {a.ArticleTags.Count}");
            //    foreach (var at in a.ArticleTags)
            //    {
            //        _logger.LogInformation($"Tag {at.TagId}");
            //        _logger.LogInformation($"Tag {at.Tag}");
            //    }
            //}
            return View(result);
        }

        [HttpGet]
        public IActionResult Tag(string tagname)
        {
            return View();
        }
    }
}
