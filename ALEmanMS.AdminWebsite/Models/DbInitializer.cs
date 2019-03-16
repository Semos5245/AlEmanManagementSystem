using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            // Insert the Units 
            context.Units.Add(new ALEmanMS.Models.Unit
            {
                Name = "كيلو",
                UnitId = Guid.NewGuid().ToString()
            });

            context.Units.Add(new ALEmanMS.Models.Unit
            {
                Name = "دزينة",
                UnitId = Guid.NewGuid().ToString()
            });

            context.Units.Add(new ALEmanMS.Models.Unit
            {
                Name = "عدد",
                UnitId = Guid.NewGuid().ToString()
            });

            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                Name = "جوي",
                JourneyTypeId = Guid.NewGuid().ToString()
            });

            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                Name = "بحري",
                JourneyTypeId = Guid.NewGuid().ToString()
            });

            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                Name = "بري",
                JourneyTypeId = Guid.NewGuid().ToString()
            });

            // Insert Roles 
            context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin"
            });

            context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Editor"
            });

            context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Secretary"
            });

            context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Accountant"
            });

            context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SaudiAdmin"
            });

            // Add the journey types 
            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                JourneyTypeId = Guid.NewGuid().ToString(),
                Name = "بري"
            });

            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                JourneyTypeId = Guid.NewGuid().ToString(),
                Name = "بحري"
            });

            context.JourneyTypes.Add(new ALEmanMS.Models.JourneyType
            {
                JourneyTypeId = Guid.NewGuid().ToString(),
                Name = "جوي"
            });

            context.SaveChanges();

            base.Seed(context);
        }

    }
}