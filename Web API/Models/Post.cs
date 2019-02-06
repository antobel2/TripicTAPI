
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

        [MaxLength(250)]
        public string Text { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public bool IsValid { get; set; }

        [Range(minimum: 0, maximum: 25)]
        public int PicNumber { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual List<Picture> Pictures { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Post()
        {
            Date = DateTime.Now.ToString("dd/MM/yy H:mm");
        }
    }

    public class PostDTO
    {
        //[Required]
        //public int ActivityId { get; set; }
        //[Required]
        //public int UserId { get; set; }
        public int Id { get; set; }
        public List<PictureDTO> PicturesDTO { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
    }

    public class CreatePostDTO
    {
        //MaxLenght: Indique le maximum de caractères permis
        [MaxLength(250)]
        public string Text { get; set; }
        //Range: Indique le maximum de photos permis
        [Range(minimum: 0, maximum: 25)]
        public int PicCount { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
    }
}