using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessObjects
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Title { get; set; }

        public string ContentRaw { get; set; }

        public string ContentHtml { get; set; }

        public string Summary { get; set; }

        [DisplayFormat(DataFormatString = "{0:ddd, MMM dd yyyy 'at' H:m}")]
        public DateTime DatePosted { get; set; }

        [DisplayFormat(DataFormatString = "{0:ddd, MMM dd yyyy 'at' H:m}")]
        public DateTime LastUpdate { get; set; }

        public Status Status { get; set; }

        public ApplicationUser User { get; set; }

        //public byte[] CoverImage { get; set; }
        //public byte[] ThumbnailImage { get; set; }

        public List<Bookmark> BookmarkedUsers { get; set;}

       // public List<Tag> Tags { get; set; }
        // public ICollection<Tag> Tags { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }


    }
}
