using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ALEmanMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ALEmanMS.AdminWebsite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        public string FullName { get => FirstName + " " + LastName; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // Add the tables here 
        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupingGroup> GroupingGroups { get; set; }

        public DbSet<Destination> Destinations { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Journey> Journeys { get; set; }

        public DbSet<Package> Pakcages { get; set; }

        public DbSet<ALEmanMS.Models.Message> Messages { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<BillItem> BillItems { get; set; }

        public DbSet<BillSender> BillSenders { get; set; }

        public DbSet<SenderCompany> SenderCompanies { get; set; }

        public DbSet<ReceiverCompany> ReceiverCompanies { get; set; }

        public DbSet<UserActivity> UserActivies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<PersonContact> PersonContacts { get; set; }

        public DbSet<Unit> Units { get; set; }

        // Additions 

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
    }
}