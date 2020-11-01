using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObjects
{
    public class Bookmark
    {
        [Required]
        public Guid ArticleID { get; set; }
        [Required]
        public Guid UserID { get; set; }
    }
}
