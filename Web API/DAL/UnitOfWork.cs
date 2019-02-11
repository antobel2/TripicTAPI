using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web_API.Models;

namespace Web_API.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private GenericRepository<Post> postRepo;
        private GenericRepository<Activity> activityRepo;
        private GenericRepository<Picture> pictureRepo;
        private GenericRepository<Trip> tripRepo;

        public UnitOfWork()
        {
            
        }

        public GenericRepository<Post> PostRepository
        {
            get
            {
                if (postRepo == null)
                {
                    postRepo = new GenericRepository<Post>(context);
                }
                return postRepo;
            }
        }

        public GenericRepository<Activity> ActivityRepository
        {
            get
            {
                if (activityRepo == null)
                {
                    activityRepo = new GenericRepository<Activity>(context);
                }
                return activityRepo;
            }
        }
        public GenericRepository<Picture> PictureRepository
        {
            get
            {
                if (pictureRepo == null)
                {
                    pictureRepo = new GenericRepository<Picture>(context);
                }
                return pictureRepo;
            }
        }

        public GenericRepository<Trip> TripRepository
        {
            get
            {
                if (tripRepo == null)
                {
                    tripRepo = new GenericRepository<Trip>(context);
                }
                return tripRepo;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}