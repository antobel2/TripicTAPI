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
    public class TripsController : ApiController
    {
        private UnitOfWork uow = new UnitOfWork();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Trips
        public IQueryable<Trip> GetTrips()
        {
            return db.Trips;
        }

        // GET: api/Trips/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult GetTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        // Post: api/CreateTrip
        [Route("api/Trip/CreateTrip")]
        [HttpPost]
        public HttpResponseMessage CreateTrip([FromBody]CreateTripDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var tripResults = uow.TripRepository.dbSet.ToArray();
            value.Name = value.Name.Trim();
            //var tripResults = db.Trips.ToList();
            foreach (Trip i in tripResults)
            {
                if (i.Name == value.Name)
                {
                    value.Name += " (" + DateTime.Now + ")";
                }
            }

            Trip trip = new Trip(value.Name);

            uow.TripRepository.Insert(trip);

            //db.Trips.Add(trip);
            //db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Trip/getTripsForUser")]
        [HttpGet]
        [ResponseType(typeof(List<TripDTO>))]
        public HttpResponseMessage GetTripsForUser()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<TripDTO> trips = new List<TripDTO>();

            foreach (Trip t in db.Trips)
            {
                TripDTO tripDTO = new TripDTO()
                {
                    Id = t.Id,
                    Name = t.Name
                };
                trips.Add(tripDTO);
            }

            return Request.CreateResponse(trips);
        }

        // PUT: api/Trips/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.Id)
            {
                return BadRequest();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        // POST: api/Trips
        [ResponseType(typeof(Trip))]
        public IHttpActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trips.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.Id }, trip);
        }

        // DELETE: api/Trips/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult DeleteTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            db.Trips.Remove(trip);
            db.SaveChanges();

            return Ok(trip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TripExists(int id)
        {
            return db.Trips.Count(e => e.Id == id) > 0;
        }
    }
}