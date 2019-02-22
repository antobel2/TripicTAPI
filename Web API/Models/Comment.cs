using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }

    public class CommentDTO
    {
        public string Text { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public CommentDTO(Comment c)
        {
            Date = c.Date.ToString("dd/MM/yy H:mm");
            Name = c.User.FirstName + " " + c.User.LastName;
            Text = c.Text;
        }
    }

    public class CreateCommentDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Text { get; set; }
        [Required(AllowEmptyStrings = false)]
        public int PostId { get; set; }
    }
}