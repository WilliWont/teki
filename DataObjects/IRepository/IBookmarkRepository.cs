using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects.IRepository
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        IQueryable<Bookmark> GetBookmarks(Article article, ApplicationUser user);
        IQueryable<Bookmark> GetBookmarks(Article article);
        IQueryable<Bookmark> GetBookmarks(ApplicationUser user);

    }
}
