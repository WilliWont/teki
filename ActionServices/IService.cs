using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.AspNetCore.Http;

namespace ActionServices
{
    public interface IService
    {
        // Article Services
        Article GetArticle(Guid id);
        IEnumerable<Article> GetAllArticle();
        void AddArticle(Article article);
        IQueryable<Article> GetArticleWithUserID(ApplicationUser user);
        bool UpdateArticle(Article article);
        // Status Service
        Status GetStatus(string name);
        Task<bool> Commit();
        void GetImage(out byte[] img, HttpRequest req);
    }
}
