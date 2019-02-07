using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class CitiesController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext(); 

        // GET: Cities
        public ActionResult Index()
        {
            CityViewModel model = new CityViewModel
            {
                Cities = db.Cities.ToList(),
                Destinations = db.Destinations.ToList()
            };

            return View(model);
        }

        // POST: Cities/Create
        [HttpPost]
        public ActionResult Create(string name, int? cityOrder, string destinationId)
        {
            // Validate the fields 
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(destinationId) || cityOrder == null)
                return new HttpStatusCodeResult(406);

            // Check the destination 
            var destination = db.Destinations.Find(destinationId);
            if (destination == null)
                return new HttpStatusCodeResult(404);
            
            name = name.Trim();

            // Create the new city 
            var city = new City
            {
                CityId = Guid.NewGuid().ToString(), 
                Name = name,
                CityOrder = cityOrder.Value,
                Destination = destination
            };

            db.Cities.Add(city);
            db.SaveChanges();

            return Json(new
            {
                Id = city.CityId,
                city.Name,
                Destination = city.Destination.Name,
                city.CityOrder
            }, JsonRequestBehavior.AllowGet); 
        }

        // Method to get the details of the city 
        public ActionResult Details(string id)
        {
            // Validate the id 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Check the city 
            var city = db.Cities.Find(id);

            if (city == null)
                return new HttpStatusCodeResult(404);

            return Json(new
            {
                city.Name,
                Destination = city.Destination.Name,
                city.DestinationId,
                city.CityOrder,
                city.CityId
            }, JsonRequestBehavior.AllowGet); 
        }

        // Method to edit a city 
        [HttpPost]
        public ActionResult Edit(string cityId, string newName, int? cityOrder, string destinationId)
        {
            // Validate arguments 
            if (string.IsNullOrEmpty(cityId) || string.IsNullOrEmpty(newName) || cityOrder == null || string.IsNullOrEmpty(destinationId))
                return new HttpStatusCodeResult(406);

            // Get the city 
            var city = db.Cities.Find(cityId);

            if (city == null)
                return new HttpStatusCodeResult(404);

            // Get the destination 
            var destination = db.Destinations.Find(destinationId); 
            if(destination == null)
                return new HttpStatusCodeResult(404);

            newName = newName.Trim();
            // Edit the city 
            city.Name = newName;
            city.CityOrder = cityOrder.Value;
            city.Destination = destination;

            db.SaveChanges();

            return Json(new
            {
                city.Name,
                city.DestinationId,
                city.CityOrder,
                Destination = city.Destination.Name,
                city.CityId
            }, JsonRequestBehavior.AllowGet); 
       
        }
        //DELETE: Cities/Delete/fd8oash7f8a

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var city = db.Cities.Find(id);

            if (city == null)
            {
                return new HttpStatusCodeResult(404);
            }

            db.Cities.Remove(city);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}