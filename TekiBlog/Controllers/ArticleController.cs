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

namespace TekiBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly IService _service;

        public ArticleController(UserManager<ApplicationUser> userManager,
            ILogger<ArticleController> logger,
            IService service)
        {
            _userManager = userManager;
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            // TODO: REMOVE LATER, FOR TEST ONLY

            List<Article> articles = _service.GetAllArticle().ToList();

            return View(articles);
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            if (id == null)
            {
                _logger.LogInformation("Id is null");
                return NotFound();
            }

            // TODO: REFACTOR THIS 
            var article = _service.GetArticle(id);
            if (article == null)
            {
                _logger.LogInformation("Article is null");
                return NotFound();
            }

            //article.User = _context.Users.First(m => m.Id == article.User);

            return View(article);
        }

        public async Task<IActionResult> Editor(Guid id)
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostArticle(CreateArticleViewModel article)
        {
            // TODO: VALIDATE POST ARTICLE

            // TODO: ADD COVER IMAGE FOR ARTICLE

            // TODO: ADD TAGS TO ARTICLS

            // TODO: REFACTOR POST ARTICLE

            // TODO: COMPRESS POST ARTICLE IMG

            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            // Create active status for this post
            Status active = _service.GetStatus("Active") ;
            
            string html = article.ArticleContent;

            // TODO: switch raw to tinymce function
            string raw = article.ArticleRaw;

            Console.WriteLine("title: "+article.Title);

            // Create article model to insert to Database
            Article articleModel = new Article
            {
                Title = article.Title,
                Summary = article.Summary,
                DatePosted = DateTime.UtcNow,
                CurrentVote = 0,
                ContentHtml = html,
                ContentRaw = raw,
                Status = active,
                User = user
            };


            if (ModelState.IsValid)
            {
                _service.AddArticle(articleModel);
                if (await _service.Commit())
                {
                    Console.WriteLine("added ID:" + articleModel.ID);
                }
                else
                {
                    return View(article);
                }
                
                // return to article view
                // return RedirectToAction(nameof(Index));
            }

            // return to home page
            return RedirectToAction("Detail", "Article", new { id = articleModel.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArticle(CreateArticleViewModel article)
        {
            // TODO: VALIDATE POST ARTICLE

            // TODO: ADD COVER IMAGE FOR ARTICLE

            // TODO: ADD TAGS TO ARTICLS

            // TODO: REFACTOR POST ARTICLE

            // TODO: COMPRESS POST ARTICLE IMG

            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            // Create active status for this post
            Status active = _service.GetStatus("Active");

            string html = article.ArticleContent;

            // TODO: switch raw to tinymce function
            string raw = article.ArticleRaw;

            Console.WriteLine("title: " + article.Title);

            // Create article model to insert to Database
            Article articleModel = new Article
            {
                ID = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                DatePosted = DateTime.UtcNow,
                CurrentVote = 0,
                ContentHtml = html,
                ContentRaw = raw,
                Status = active,
                User = user
            };


            if (ModelState.IsValid)
            {
                _service.UpdateArticle(articleModel);
                if (await _service.Commit())
                {
                    Console.WriteLine("updated ID:" + articleModel.ID);
                }
                else
                {
                    return View(article);
                }

                // return to article view
                // return RedirectToAction(nameof(Index));
            }

            // return to home page
            return RedirectToAction("Detail", "Article", new { id = articleModel.ID });
        }
    }
}
