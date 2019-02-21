using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Web_API.Models.CustomValidationErrors;

namespace Web_API.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<Activity> Activities { get; set; }
        public virtual List<SeenTrips> SeenTrips { get; set; }

        public Trip()
        {

        }
        public Trip(string name)
        {
            Name = name;
            Users = new List<ApplicationUser>();
            SeenTrips = new List<SeenTrips>();
        }
    }

    public class TripDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Boolean Seen { get; set; }

        public TripDTO toTripDTO(Trip trip)
        {
            TripDTO result = new TripDTO
            {
                Id = trip.Id,
                Name = trip.Name
            };
            return result;
        }
    }

    public class CreateTripDTO
    {
        public string Name { get; set; }
    }

    public class InviteUserToTripDTO
    {
        [InviteUserToTripUserIdsError(ErrorMessage ="Le champ UserIds a ne doit pas être vide")]
        public List<string> UserIds { get; set; }
        [Required]
        public int TripId { get; set; }
    }
}