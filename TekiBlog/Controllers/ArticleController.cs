using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Quill.Delta;
using TekiBlog.Data;
using TekiBlog.ViewModels;

namespace TekiBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ArticleController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _context.Articles
            .FirstOrDefault(m => m.ID.Equals(id));
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
        public IActionResult PostArticle(CreateArticleViewModel article)
        {
            // TODO: INSERT ARTICLE IN DB

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
            //ViewData["BlogContent"] = html;

            Console.WriteLine($"summary: {article.Summary}");
            Console.WriteLine($"title: {article.Title}");

            // TODO: CHANGE REDIRECT TO JUST POSTED ARTICLE DETAIL
            return RedirectToAction("Index","Home");
        }
    }
}
