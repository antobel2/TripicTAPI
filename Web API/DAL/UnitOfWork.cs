using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web_API.Models;

namespace Web_API.DAL
{
    public class UnitOfWork
    {
        private Dictionary<Type, object> repoDict;
        private DbContext context;

        public UnitOfWork()
        {
            repoDict = new Dictionary<Type, object>();
            context = ApplicationDbContext.Create();
        }

        public IGenericRepository<T> Repo<T>() where T : class
        {
            if (!repoDict.ContainsKey(typeof(T)))
                repoDict.Add(typeof(T), new GenericRepository<T>(context));
            return repoDict[typeof(T)] as IGenericRepository<T>;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}