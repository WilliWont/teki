﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace TekiBlog.ViewModels
{
    public class CreateArticleViewModel
    {
        //[AllowHtml] // nescessary for HTML to pass through   
        [Required]
        public string ArticleContent { get; set; }

        [Required]
        public string ArticleRaw { get; set; }


        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
