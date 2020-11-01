using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessObjects
{
    public class Bookmark
    {
        [DisplayFormat(DataFormatString = "{0:ddd, MMM dd yyyy 'at' H:m}")]
        public DateTime DatePosted { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Guid ArticleID { get; set; }

        public virtual Article Article { get; set; }
    }
}
