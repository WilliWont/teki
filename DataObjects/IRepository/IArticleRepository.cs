using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataObjects.IRepository;

namespace DataObjects
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        // There are some additional methods for Article Implemetations.
        bool UpdateArticle(Article article);
        Article GetArticleInfo(Guid ID);
        IQueryable<Article> GetArticlesByID(ApplicationUser user);
        IQueryable<Article> SearchArticle(string searchValue);
        IQueryable<Article> GetArticleByStatus(string status);

        IQueryable<Article> GetArticlesForViewer(ApplicationUser user);
    }
}
