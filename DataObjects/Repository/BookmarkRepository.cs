using BusinessObjects;
using DataObjects.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataObjects.Repository
{
    public class BookmarkRepository : GenericRepository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(ApplicationDBContext context) : base(context)
        {

        }

        public IQueryable<Bookmark> GetBookmarks(Article article, ApplicationUser user)
        {
            IQueryable<Bookmark> bookmarks = _context.Bookmarks
                 .Include(a => a.User)
                 .Select(a => a)
                 .Where(a => a.User.Equals(user) && a.Article.Equals(article))
                 .OrderByDescending(a => a.DatePosted);

            return bookmarks;
        }

        public IQueryable<Bookmark> GetBookmarks(ApplicationUser user)
        {
            IQueryable<Bookmark> bookmarks = _context.Bookmarks
                 .Include(a => a.User)
                 .Select(a => a)
                 .Where(a => a.User.Equals(user))
                 .OrderByDescending(a => a.DatePosted);

            return bookmarks;
        }

        public IQueryable<Bookmark> GetBookmarks(Article article)
        {
            IQueryable<Bookmark> bookmarks = _context.Bookmarks
                 .Include(a => a.User)
                 .Select(a => a)
                 .Where(a => a.Article.Equals(article))
                 .OrderByDescending(a => a.DatePosted);

            return bookmarks;
        }

    }
}
