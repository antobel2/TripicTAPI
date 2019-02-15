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
        public string Name { get; set; }
        public int TripId { get; set; }
    }
}