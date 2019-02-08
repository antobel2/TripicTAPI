using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class Picture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Base64 { get; set; }

        public virtual Post Post { get; set; }
    }

    public class PictureWithDatabaseStorageStrategy : Picture
    {
        public byte[] Content { get; set; }
    }

    public class PictureDTO
    {
        public byte[] Content { get; set; }
        public int Id { get; set; }
    }

    public class CreatePictureDTO
    {
        [Required]
        public string Base64 { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}