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

    public class CreatePostDTO
    {
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public int UserId { get; set; }
        public List<CreatePictureDTO> CreatePicturesDTO { get; set; }
        public string Text { get; set; }
    }

    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ActivityId { get; set; }
        public List<PictureDTO> PicturesDTO { get; set; }
        public int UserId { get; set; }
    }
}