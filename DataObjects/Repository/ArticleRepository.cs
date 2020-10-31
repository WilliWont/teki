using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Repository
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        // Inheriteance from GenericRepository with ApplicationDBContext
        public ArticleRepository(ApplicationDBContext context) : base(context)
        {

        }

        public Article GetArticleInfo(Guid ID)
        {
            Article article = _context.Articles
                .Include(a => a.User)
                .Include(a => a.Status)
                .FirstOrDefault(a => (a.ID.Equals(ID) && (!a.Status.Name.Equals("Deleted"))));
            return article;
        }

        public IQueryable<Article> GetArticlesByID(ApplicationUser user)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Select(a => new Article { ID = a.ID, User = a.User, Status = a.Status, Title = a.Title })
                .Where(a => (a.User.Equals(user) && (!a.Status.Name.Equals("Deleted"))));
            return articles;
        }

        public IQueryable<Article> SearchArticle(string searchValue)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Select(a => a)
                .Where(a => a.Status.Name.Equals("Active") 
                && (a.Title.Contains(searchValue) || a.Summary.Contains(searchValue)))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        // Implement all additional methods in IArticleRepository
        public bool UpdateArticle(Article article)
        {
            _context.Articles.Update(article);

            return true;
        }
    }
}
