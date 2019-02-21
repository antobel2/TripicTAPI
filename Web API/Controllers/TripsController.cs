using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.DAL;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class TripsController : ApiController
    {
        private UnitOfWork uow = new UnitOfWork();
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Trips
        [Route("api/Trips/GetTripsForUser")]
        [Authorize]
        public List<TripDTO> GetTripsForUser()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            List<TripDTO> results = new List<TripDTO>();
            foreach (Trip trip in currentUser.Trips.OrderByDescending(t => t.Date))
            {
                TripDTO toDto = new TripDTO();
                toDto = toDto.toTripDTO(trip);
                SeenTrips seenStatus = currentUser.SeenTrips.Find(x => x.TripId == trip.Id && x.UserId == currentUser.Id);
                toDto.Seen = seenStatus.Seen;
                results.Add(toDto);
            }

            return results;
        }

        // GET: api/Trips/5
        [Authorize]
        [Route("api/Trips/GetTripById/{id}")]
        public HttpResponseMessage GetTrip(int id)
        {
            //Trouver le voyage
            Trip trip = uow.TripRepository.GetByID(id);
            if (trip == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun resultats");
            }

            //Vérifier que l'utilisateur a les droits sur le voyage
            if (!UserHasTripPermitions(trip.Id))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "L'utilisateur actuel n'a pas les droits d'accès au voyage demandé");
            }

            TripDTO result = new TripDTO();
            result = result.toTripDTO(trip);

            return Request.CreateResponse(result);
        }

        // Post: api/CreateTrip
        [Route("api/Trips/CreateTrip")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage CreateTrip([FromBody]CreateTripDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            if (value.Name.Length < 1 || value.Name.Length > 35)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le voyage doit avoir un nom comptant entre 1 et 35 caractères");
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);

            value.Name = value.Name.Trim();

            Trip trip = new Trip(value.Name);
            trip.Date = DateTime.Now;
            trip.Users.Add(currentUser);


            uow.TripRepository.Insert(trip);
            //lié le voyage crée a l'utilisateur
            currentUser.Trips.Add(trip);
            SeenTrips seen = new SeenTrips
            {
                Seen = true,
                Trip = trip,
                User = currentUser
            };
            currentUser.SeenTrips.Add(seen);
            uow.Save();


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Trips/InviteUserToTrip")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage InviteUserToTrip([FromBody]InviteUserToTripDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le model InviteUserToTripDTO n'est pas valide");
            }
            //Trouver le voyage
            Trip trip = uow.TripRepository.GetByID(value.TripId);
            if (trip == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun résultats");
            }
            //Vérifier que l'utilisateur a les droits sur le voyage
            if (!UserHasTripPermitions(trip.Id))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "L'utilisateur actuel n'a pas les droits d'accès au voyage demandé");
            }
            
            //Ajouter un utilisateur a un voyage s'il ne l'est pas deja
            foreach (string userId in value.UserIds)
            {
                ApplicationUser userToInvite = uow.UserRepository.GetByID(userId);
                if (userToInvite == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id de l'utilisateur à inviter n'a retourné aucun résultats");
                }

                if (userToInvite.Trips.FirstOrDefault(x => x.Id == trip.Id) == null)
                {
                    userToInvite.Trips.Add(trip);
                    SeenTrips seen = new SeenTrips
                    {
                        Seen = false,
                        Trip = trip,
                        User = userToInvite
                    };
                    userToInvite.SeenTrips.Add(seen);
                    
                }
            }
            uow.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Trips/GetUsersForTrip/{id}")]
        public HttpResponseMessage GetUsersForTrip(int id)
        {
            Trip trip = uow.TripRepository.GetByID(id);
            if (trip == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun résultats");
            }
            List<SignedInUserDTO> results = new List<SignedInUserDTO>();
            foreach (ApplicationUser user in trip.Users)
            {
                SignedInUserDTO a = new SignedInUserDTO()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    UUID = user.Id
                };
                results.Add(a);
            }
            return Request.CreateResponse(results);
        }

        public Boolean UserHasTripPermitions(int tripId)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            if (currentUser.Trips.FirstOrDefault(x => x.Id == tripId) == null)
            {
                return false;
            }
            return true;
        }
    }
}