using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace TekiBlog.ViewModels
{
    public class CreateArticleViewModel
    {
        // NOTE: TINYMCE-1: create model
        // Create a model for containing editor's content
        // REQUIRE .Web.Mvc namespace
        //[AllowHtml] // nescessary for HTML to pass through
        public string ArticleContent { get; set; }

        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
