using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class DriversController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public DriversController()
        {
            ViewBag.Page = "Drivers";
        }

        // GET: Drivers
        public ActionResult Index()
        {
            return View(db.People.OfType<Driver>().ToList());
        }

        //GET: Drivers/Create
        public ActionResult Create()
        {
            return View(new DriverViewModel());
        }

        //POST: Drivers/Create
        [HttpPost]
        public ActionResult Create(DriverViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldDriver = db.People.OfType<Driver>().FirstOrDefault(d => d.FirstName == model.FirstName && d.LastName == model.LastName && d.VehicleNumber == model.VehicleNumber);

                if (oldDriver != null)
                    return View(model);

                var newDriver = new Driver
                {
                    PersonId = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    CityName = model.CityName.Trim(),
                    Description = model.Description != null ? model.Description.Trim() : "",
                    VehicleNumber = model.VehicleNumber.Trim(),
                    Birthdate = model.BirthDate,
                    VehicleFrontHeight = model.VehicleFrontHeight,
                    VehicleLength = model.VehicleLength,
                    VehicleRearHight = model.VehicleRearHeight,
                    VehicleSize = model.VehicleSize
                };

                db.People.Add(newDriver);
                db.SaveChanges();

                return RedirectToAction("Index", "Drivers");
            }

            return View(model);
        }

        //GET: Drivers/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var oldDriver = db.People.OfType<Driver>().FirstOrDefault(d => d.PersonId == id);

            if (oldDriver == null)
                return HttpNotFound();

            var driver = new DriverViewModel
            {
                FirstName = oldDriver.FirstName,
                LastName = oldDriver.LastName,
                BirthDate = oldDriver.Birthdate,
                CityName = oldDriver.CityName,
                Description = oldDriver.Description,
                VehicleFrontHeight = oldDriver.VehicleFrontHeight,
                VehicleLength = oldDriver.VehicleLength,
                VehicleNumber = oldDriver.VehicleNumber,
                VehicleRearHeight = oldDriver.VehicleRearHight,
                VehicleSize = oldDriver.VehicleSize,
            };

            return View(driver);
        }

        //POST: Drivers/Edit
        [HttpPost]
        public ActionResult Edit(string id, DriverViewModel model)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            if (ModelState.IsValid)
            {
                var driver = db.People.OfType<Driver>().FirstOrDefault(d => d.PersonId == id);
                if (driver == null)
                    return HttpNotFound();

                var oldDriver = db.People.OfType<Driver>().SingleOrDefault(d => d.FirstName == model.FirstName && d.LastName == model.LastName && d.VehicleNumber == model.VehicleNumber && d.PersonId != id);
                if (oldDriver != null)
                    return View(model);

                driver.FirstName = model.FirstName.Trim();
                driver.LastName = model.LastName.Trim();
                driver.CityName = model.CityName.Trim();
                driver.Birthdate = model.BirthDate;
                driver.Description = model.Description != null ? model.Description.Trim() : "";
                driver.VehicleFrontHeight = model.VehicleFrontHeight;
                driver.VehicleLength = model.VehicleLength;
                driver.VehicleNumber = model.VehicleNumber.Trim();
                driver.VehicleRearHight = model.VehicleRearHeight;
                driver.VehicleSize = model.VehicleSize;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //POST: Drivers/Delete
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var driver = db.People.OfType<Driver>().FirstOrDefault(d => d.PersonId == id);

            if (driver == null)
                return HttpNotFound();

            db.People.Remove(driver);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}