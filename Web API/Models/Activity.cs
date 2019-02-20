using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual Trip Trip { get; set; }

        public virtual List<Post> Posts { get; set; }

        public Activity()
        {
        }
        public Activity(string name)
        {
            Name = name;
        }
    }

    public class ActivityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ActivityDTO toDto(Activity activity)
        {
            ActivityDTO results = new ActivityDTO
            {
                Id = activity.Id,
                Name = activity.Name
            };
            return results;
        }
    }

    public class CreateActivityDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "L'activité doit avoir un nom comptant entre 1 et 35 caractères")]
        [MinLength(1, ErrorMessage = "L'activité doit avoir un nom comptant au moins 1 caractère")]
        [MaxLength(35, ErrorMessage = "L'activité doit avoir un nom comptant au plus 35 caractère")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Il est impossible de créer une activité sans l'associer à un voyage")]
        public int TripId { get; set; }
    }
}