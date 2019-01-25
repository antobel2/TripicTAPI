using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        
        public virtual Activity Activity { get; set; }
        
        public virtual List<Picture> Pictures { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Post()
        {

        }

        public Post(string text)
        {
            Text = text;
        }
    }

    public class newPost
    {
        [Required]
        public List<Picture> Pictures { get; set; }
        public string Text { get; set; }
    }
}