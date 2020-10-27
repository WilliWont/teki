using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .FirstOrDefault(a => a.ID.Equals(ID));
            return article;
        }

        public IQueryable<Article> GetArticlesByID(ApplicationUser user)
        {
            IQueryable<Article> articles = null;
            articles = _context.Articles
                .Include(a => a.User)
                .Include(a => a.Status)
                .Select(a => new Article { ID = a.ID, Status = a.Status, Title = a.Title , User = a.User })
                .Where(a => a.User.Equals(user));
            return articles;
        }

        // Implement all additional methods in IArticleRepository
        public bool UpdateArticle(Article article)
        {

            throw new NotImplementedException();
        }

    }
}
