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
            // This query return article
            //var query = (from article in _context.Articles
            //             join user in _context.Users on article.User equals user
            //             join status in _context.Statuses on article.Status equals status
            //             where article.ID.Equals(ID) && !article.Status.Name.Equals("Deleted")
            //             select new Article
            //             {
            //                 User = user,
            //                 Status = status
            //             }.SetArticleInfo(article)
            //             ).First();
            //// this query return article tag of this article
            //var articleTags = (from articleTag in _context.ArticleTags
            //                   join tag in _context.Tags on articleTag.TagId equals tag.Id
            //                   where articleTag.ArticleId.Equals(ID) && tag.IsActive
            //                   select new ArticleTag
            //                   {
            //                       Tag = tag,
            //                       TagId = tag.Id
            //                   }
            //                   ).ToList();
            //Article rs = null;
            //rs = query;
            //rs.ArticleTags = articleTags;

            IQueryable<Article> articles = _context.Articles
                .Include(a => a.User)
                .Include(a => a.Status)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                .Select(a => new Article
                {
                    ID = a.ID,
                    Title = a.Title,
                    Summary = a.Summary,
                    ContentHtml = a.ContentHtml,
                    ContentRaw = a.ContentRaw,
                    DatePosted = a.DatePosted,
                    User = a.User,
                    Status = a.Status,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.ID.Equals(ID) && (!a.Status.Name.Equals("Deleted")));
            if (articles.Any())
            {
                return articles.FirstOrDefault();
            }
            return null;
        }

        public IQueryable<Article> GetArticlesByUser(ApplicationUser user)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                .Select(a => new Article
                {
                    ID = a.ID,
                    User = a.User,
                    Status = a.Status,
                    Title = a.Title,
                    DatePosted = a.DatePosted,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.User.Equals(user) && (!a.Status.Name.Equals("Deleted")))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public IQueryable<Article> GetArticlesForViewer(ApplicationUser user)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                .Select(a => new Article
                {
                    ID = a.ID,
                    User = a.User,
                    Status = a.Status,
                    Summary = a.Summary,
                    Title = a.Title,
                    DatePosted = a.DatePosted,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.Status.Name.Equals("Active") && a.User.Equals(user))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public IQueryable<Article> SearchArticle(string searchValue)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                .Select(a => new Article
                {
                    ID = a.ID,
                    User = a.User,
                    Status = a.Status,
                    Summary = a.Summary,
                    ContentRaw = a.ContentRaw,
                    Title = a.Title,
                    DatePosted = a.DatePosted,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.Status.Name.Equals("Active")
                            && (a.Title.Contains(searchValue)
                            || a.Summary.Contains(searchValue)
                            || a.ContentRaw.Contains(searchValue)))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public IQueryable<Article> GetArticleByStatus(string status)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                //.Select(a => a)
                .Select(a => new Article
                {
                    ID = a.ID,
                    Title = a.Title,
                    Summary = a.Summary,
                    //ContentHtml = a.ContentHtml,
                    //ContentRaw = a.ContentRaw,
                    DatePosted = a.DatePosted,
                    //CurrentVote = a.CurrentVote,
                    User = a.User,
                    Status = a.Status,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.Status.Name.Equals(status))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public bool UpdateArticle(Article article)
        {
            _context.Articles.Update(article);

            return true;
        }

        public IQueryable<Article> GetArticlesForAdmin()
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public IQueryable<Article> GetArtcilesByTag(int tagid)
        {
            List<Guid> articleIDs = (from articleTag in _context.ArticleTags
                                     join tag in _context.Tags on articleTag.TagId equals tag.Id
                                     where tag.Id == tagid && tag.IsActive
                                     select articleTag.ArticleId
                               ).ToList();
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Include(a => a.ArticleTags)
                .ThenInclude(articleTag => articleTag.Tag)
                .Select(a => new Article
                {
                    ID = a.ID,
                    Title = a.Title,
                    Summary = a.Summary,
                    ContentHtml = a.ContentHtml,
                    ContentRaw = a.ContentRaw,
                    DatePosted = a.DatePosted,
                    User = a.User,
                    Status = a.Status,
                    ArticleTags = a.ArticleTags.Where(at => at.Tag.IsActive).ToList()
                })
                .Where(a => a.Status.Name.Equals("Active") && articleIDs.Contains(a.ID))
                .OrderByDescending(a => a.DatePosted);
            return articles;
        }

        public IQueryable<Article> GetUserDrafts(ApplicationUser user)
        {
            IQueryable<Article> articles = _context.Articles
                .Include(a => a.Status)
                .Include(a => a.User)
                .Select(a => new Article { ID = a.ID, User = a.User, Status = a.Status, Title = a.Title, LastUpdate = a.LastUpdate })
                .Where(a => a.User.Equals(user) && ( a.Status.Name.Equals("Draft") ))
                .OrderByDescending(a => a.LastUpdate);
            return articles;
        }
    }
}
