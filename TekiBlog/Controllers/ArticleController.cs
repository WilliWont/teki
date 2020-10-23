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
using TekiBlog.Data;
using TekiBlog.Models;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly ApplicationDBContext _context;

        public ArticleController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signinManager,
            ApplicationDBContext context)
        {
            _userManager = userManager;
            _signinManager = signinManager;
           _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _context.Articles.FirstOrDefault(m => m.ID.Equals(id));
            if (article == null)
            {
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

            string userId = _userManager.GetUserId(HttpContext.User);
            //Status active = new Status { ID = 2, Name = "Active" };
            //ApplicationUser user = new ApplicationUser { Id = userId };

            string raw = Regex.Replace(html, "<.*?>", String.Empty);

            SHA256 mySHA256 = SHA256.Create();
            byte[] bytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString() + userId));

            // Convert byte array to a string   
            StringBuilder articleID = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                articleID.Append(bytes[i].ToString("x2"));
            }

            Article articleModel = new Article
            {
                ID = articleID.ToString(),
                Title = article.Title,
                Summary = article.Summary,
                DatePosted = DateTime.UtcNow,
                CurrentVote = 0,
                ContentHtml = html,
                ContentRaw = raw,
                StatusID = 2,
                UserID = userId
            };


            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                Console.WriteLine("added ID:"+articleModel.ID);
                // return to article view
                // return RedirectToAction(nameof(Index));
            }

            // return to home page
            return RedirectToAction("Detail","Article", new {id= articleModel.ID });
        }

    }
}
