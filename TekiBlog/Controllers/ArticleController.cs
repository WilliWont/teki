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
using Quill.Delta;
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
            return View();
        }

        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            if (id == null)
            {
                _logger.LogInformation("Id is null");
                return NotFound();
            }

            var article = _context.Articles.FirstOrDefault(m => m.ID.Equals(id));
            if (article == null)
            {
                _logger.LogInformation("Article is null");
                return NotFound();
            }

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

            // TODO: REFACTOR POST ARTICLE

            // TODO: COMPRESS POST ARTICLE IMG

            #region article content
            string content = article.ArticleContent;
            content = content.Substring(7);
            int endI = content.Length - 1;
            content = content.Substring(0, endI);
            Console.WriteLine($"contentPROC: {content}");

            var deltaOps = JArray.Parse(content);
            Console.WriteLine($"contentJSON: {deltaOps}");
            var htmlConverter = new HtmlConverter(deltaOps);
            string html = htmlConverter.Convert();
            Console.WriteLine($"html: {html}");
            #endregion

            // Get user in current context
            var user = await _userManager.GetUserAsync(User);
            // Create active status for this post
            Status active = _context.Statuses.First(x => x.Name.Equals("Active"));
            

            string raw = Regex.Replace(html, "<.*?>", String.Empty);

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
