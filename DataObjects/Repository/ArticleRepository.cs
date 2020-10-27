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

        // Implement all additional methods in IArticleRepository
        public bool UpdateArticle(Article article)
        {

            _context.Articles.Update(article);

            return true;
        }
        
    }
}
