using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObjects
{
    public class ArticleTag
    {
        public int TagId { get; set; }
        public Guid ArticleId { get; set; }
        public Tag Tag { get; set; }
        public Article Article { get; set; }
    }
}
