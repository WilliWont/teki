using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekiBlog.ViewModels
{
    public class BookmarkViewModel
    {
        public IQueryable<Bookmark> Bookmarks { get; set;}
    }
}
