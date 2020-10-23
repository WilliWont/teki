using System;
using System.ComponentModel.DataAnnotations;

namespace TekiBlog.Models
{
    public class Article
    {
        public Guid ID { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public string ContentRaw { get; set; }

        public string ContentHtml { get; set; }

        public string Summary { get; set; }

        public DateTime DatePosted { get; set; }

        public int CurrentVote { get; set; }

        public int StatusID { get; set;}
        public Status Status { get; set; }

        public string UserID { get; set;}
        public ApplicationUser User { get; set; }
    }
}
