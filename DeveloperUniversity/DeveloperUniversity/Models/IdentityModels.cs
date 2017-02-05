using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using DeveloperUniversity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DeveloperUniversity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
#if DEBUG
            ////This will create database if one doesn't exist.
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            ////This will drop and re-create the database if model changes.
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
#endif


        }

        //Note: Adding ApplicationDbContext inside the ASP .NET Idenity Context
        //      creates application model tables when logging into site (when context runs)

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Note: Could do mapping here for each DbSet instead of creating separate mapping files.
            //modelBuilder.Entity<Student>().HasKey(p => p.Id);

            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        //We must add DbSets here for each table we want Entity Framework to generate.
        //Then we must add a mapping file to tell Entity Framework HOW we want those tables generated.
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Absence> Absences { get; set; }
    }
}