using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TekiBlog.Models
{
    public class Article
    {
        public string ID { get; set; }
        [StringLength(200)]
        public string Tittle { get; set; }
        public string ContentRaw { get; set; }
        public string ContentHtml { get; set; }
        public string Sumary { get; set; }
        public DateTime DatePosted { get; set; }
        public int CurrentVote { get; set; }
        public Status status { get; set; }
        public ApplicationUser user { get; set; }
    }
}
