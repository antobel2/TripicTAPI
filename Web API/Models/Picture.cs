﻿using System;
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
        public string FilenameWithExtension { get; set; }
        [Required]
        public string MimeType { get; set; }

        public virtual Post Post { get; set; }
    }
    public class PictureWithNetworkStorageStrategy : Picture
    {
        public Guid StorageKey { get; set; }
    }

    public class PictureWithDatabaseStorageStrategy : Picture
    {
        public byte[] Content { get; set; }
    }
}