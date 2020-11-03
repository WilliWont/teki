using BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ValidationUtilities.Util;

namespace TekiBlog.ViewModels
{
    public class CreateArticleViewModel
    {
        public Guid Id{get; set;}

        //[AllowHtml] // nescessary for HTML to pass through   
        [Required]
        public string ArticleContent { get; set; }

        [Required]
        [StringLength(5000,MinimumLength = 128)]
        public string ArticleRaw { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 8)]
        public string Title { get; set; }

        [StringLength(256, MinimumLength = 8)]
        public string Summary { get; set; }

        public byte[] CoverImage { get; set;}

        public byte[] ThumbnailImage { get; set; }

        public Status Status { get; set;}
    }
}
