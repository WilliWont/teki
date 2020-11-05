using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjects
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Navigation properties
        //public ICollection<Article> Articles { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }
    }
}
