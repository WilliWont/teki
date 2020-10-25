using System;
using System.Collections.Generic;
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
    }
}
