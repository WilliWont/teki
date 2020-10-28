using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace TekiBlog.ViewModels
{
    public class CreateArticleViewModel
    {
        public Guid Id{get; set;}

        //[AllowHtml] // nescessary for HTML to pass through   
        [Required]
        public string ArticleContent { get; set; }

        [Required]
        public string ArticleRaw { get; set; }

        [Required]
        public string Title { get; set; }

        public string Summary { get; set; }

        public int TITLE_MAX_LEN { get; } = 10;
        public int SUMMARY_MAX_LEN { get { return 10; } }
        public int CONTENT_MAX_LEN { get { return 10; } }
    }
}
