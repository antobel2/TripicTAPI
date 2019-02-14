﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class ActivitiesController : ApiController
    {
        private UnitOfWork uow = new UnitOfWork();
        // private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Activities
        [Route("api/Activities/GetActivitiesForTrip/{id}")]
        [Authorize]
        public HttpResponseMessage GetActivitiesForTrip(int id)
        {
            //Trouver le voyage
            Trip trip = uow.TripRepository.GetByID(id);
            if (trip == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun résultats");
            }

            //Verifier que l'utilisateur à les droits d'accès au voyage
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            if (currentUser.Trips.FirstOrDefault(x => x.Id == id) == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "L'utilisateur n'à pas les droits d'accès sur le voyage");
            }
            
            List<ActivityDTO> results = new List<ActivityDTO>();
            foreach (Activity activity in trip.Activities)
            {
                ActivityDTO res = new ActivityDTO();
                res = res.toDto(activity);
                results.Add(res);
            }
            return Request.CreateResponse(results);
        }

        [Route("api/Activities/GetActivityById/{id}")]
        public HttpResponseMessage GetActivityById(int id)
        {
            //Trouver l'activité
            Activity activity = uow.ActivityRepository.GetByID(id);
            if (activity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id de l'activité n'a retourné aucun resultats");
            }
            ActivityDTO result = new ActivityDTO();
            result = result.toDto(activity);
            
            return Request.CreateResponse(result);
        }

        

        // POST: api/Activities
        [HttpPost]
        [Authorize]
        [Route("api/Activities/CreateActivity")]
        public HttpResponseMessage CreateActivity([FromBody]CreateActivityDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            Activity activity = new Activity(value.Name.Trim());

            var activities = uow.ActivityRepository.dbSet.ToArray();
            foreach (Activity act in activities)
            {
                if (act.Name == value.Name)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Une activité avec ce nom existe déjà dans le voyage");
                }
            }

            activity.Trip = uow.TripRepository.GetByID(value.TripId);
            activity.Posts = new List<Post>();
            
            uow.ActivityRepository.Insert(activity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}