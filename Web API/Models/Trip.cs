using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<Activity> Activities { get; set; }

        public Trip()
        {

        }
        public Trip(string name)
        {
            Name = name;
        }
    }

    public class TripDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

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
        public List<string> UserId { get; set; }
        public int TripId { get; set; }
    }
}