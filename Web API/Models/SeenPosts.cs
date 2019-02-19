using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class SeenPosts
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }

        public bool Seen { get; set; }

    }
}