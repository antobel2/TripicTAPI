
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    //TODO: Décommenter
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool IsValid { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual List<Picture> Pictures { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Post()
        {
            Date = DateTime.Now;
        }
    }

    public class CreatePostDTO
    {
        //[Required]
        //public int ActivityId { get; set; }
        //[Required]
        //public int UserId { get; set; }
        public List<PictureDTO> PicturesDTO { get; set; }
        public string Text { get; set; }
    }

    public class PostDTO
    {
        public string Text { get; set; }
        public int PictureNumber { get; set; }
        //public int ActivityId { get; set; }
        //public int UserId { get; set; }
    }
}