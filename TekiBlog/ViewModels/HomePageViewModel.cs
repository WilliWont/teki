using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TekiBlog.ViewModels
{
    public class HomePageViewModel
    {
        public PaginatedList<Article> Articles { get; set;}
        public List<Bookmark> UserBookmarks { get; set;}
    }
}
