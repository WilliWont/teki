using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace ActionServices
{
    public interface IService
    {
        // Article Services
        Article GetArticle(Guid id);
        IEnumerable<Article> GetAllArticle();
        void AddArticle(Article article);

        // Status Service
        Status GetStatus(string name);

        Task<bool> Commit();
    }
}
