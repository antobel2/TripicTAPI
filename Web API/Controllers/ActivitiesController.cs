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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Activities
        public IQueryable<Activity> GetActivities()
        {
            return db.Activities;
        }

        // GET: api/Activities/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult GetActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/Activities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActivity(int id, Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.Id)
            {
                return BadRequest();
            }

            db.Entry(activity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Activities
        [HttpPost]
        [Route("api/Activity/CreateActivity")]
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

            activity.Trip = uow.TripRepository.dbSet.Find(value.TripId);
            // activity.Trip = db.Trips.Find(value.TripId);
            activity.Posts = new List<Post>();

            //uow.ActivityRepository.dbSet.Add(activity);
            //uow.ActivityRepository.context.SaveChanges();
            //db.Activities.Add(activity);
            //db.SaveChanges();
            uow.ActivityRepository.Insert(activity);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [Route("api/Activity/getActivitiesForTrip/{id}")]
        [HttpGet]
        [ResponseType(typeof(List<TripDTO>))]
        public HttpResponseMessage GetActivitiesForTrip(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Trip trip = uow.TripRepository.GetByID(id);
            List<ActivityDTO> activityDTOs = new List<ActivityDTO>();
            foreach (Activity a in trip.Activities)
            {
                ActivityDTO act = new ActivityDTO
                {
                    Id = a.Id,
                    Name = a.Name
                };
                activityDTOs.Add(act);
            }
            return Request.CreateResponse(activityDTOs);
        }

        // DELETE: api/Activities/5
        [ResponseType(typeof(Activity))]
        public IHttpActionResult DeleteActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return NotFound();
            }

            db.Activities.Remove(activity);
            db.SaveChanges();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActivityExists(int id)
        {
            return db.Activities.Count(e => e.Id == id) > 0;
        }
    }
}