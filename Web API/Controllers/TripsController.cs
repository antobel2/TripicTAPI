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
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            if (currentUser.Trips.FirstOrDefault(x => x.Id == trip.Id) == null)
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

            var tripResults = uow.TripRepository.dbSet.ToArray();
            value.Name = value.Name.Trim();

            Trip trip = new Trip(value.Name);
            trip.Date = DateTime.Now;

            uow.TripRepository.Insert(trip);
            //lié le voyage crée a l'utilisateur
            currentUser.Trips.Add(trip);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Trips/InviteUserToTrip")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage InviteUserToTrip([FromBody]InviteUserToTripDTO value)
        {
            //Trouver le voyage
            Trip trip = uow.TripRepository.GetByID(value.TripId);
            if (trip == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun résultats");
            }

            //Vérifier que l'utilisateur a les droits sur le voyage
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            if (currentUser.Trips.FirstOrDefault(x => x.Id == value.TripId) == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "L'utilisateur actuel n'a pas les droits d'accès au voyage demandé");
            }

            //Ajouter un utilisateur a un voyage s'il ne l'est pas deja
            foreach (string userIds in value.UserId)
            {
                ApplicationUser userToInvite = uow.UserRepository.GetByID(value.UserId);
                // a verifier ce qui arrive si un seul ne fonctionne pas
                if (userToInvite == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "l'id de l'utilisateur à inviter n'a retourné aucun résultats");
                }

                if (userToInvite.Trips.FirstOrDefault(x => x.Id == trip.Id) == null)
                {
                    userToInvite.Trips.Add(trip);
                }
            }
            
            

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}