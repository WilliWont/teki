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
using Microsoft.Extensions.Logging;

namespace TekiBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ArticleController> _logger;
        public ArticleController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            ApplicationDBContext context,
            ILogger<ArticleController> logger)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // TODO: REMOVE LATER, FOR TEST ONLY

            List<Article> articles = _context.Articles.ToList();

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
            var article = _context.Articles.FirstOrDefault(m => m.ID.Equals(id) && m.Status.ID == 1);
            if (article == null)
            {
                _logger.LogInformation("Article is null");
                return NotFound();
            }

            //article.User = _context.Users.First(m => m.Id == article.User);

            return View(article);
        }

        public IActionResult Editor()
        {
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
            Status active = _context.Statuses.First(x => x.Name.Equals("Active"));
            
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
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                Console.WriteLine("added ID:" + articleModel.ID);
                // return to article view
                // return RedirectToAction(nameof(Index));
            }

            // return to home page
            return RedirectToAction("Detail", "Article", new { id = articleModel.ID });
        }

    }
}
