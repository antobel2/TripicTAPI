using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web_API.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            :base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
    }

    public class Init : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //TODO: Changer les valeurs hardcodées
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var defaultUser = 
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "a@b.c",
                    Email = "a@b.c"
                };

            var defaultActivity = new Activity("Super Activité");
            context.Users.Add(defaultUser);
            context.Activities.Add(defaultActivity);
            context.SaveChanges();
            userManager.AddPassword(defaultUser.Id, "Passw0rd");
        }
    }
}