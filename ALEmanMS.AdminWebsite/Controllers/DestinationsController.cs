using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class DestinationsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        //Get the destinations
        public DestinationsController()
        {
            ViewBag.Page = "Destinations";
        }

        // GET: Destinations
        public ActionResult Index()
        {
            return View(db.Destinations.ToList());
        }

        // POST: Destinations/Create 
        [HttpPost]
        public ActionResult Create(string destinationName)
        {
            // Validate the argument 
            if (string.IsNullOrEmpty(destinationName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }

            // Check if the destination is existed or not 
            destinationName = destinationName.Trim();

            var destination = db.Destinations.SingleOrDefault(d => d.Name == destinationName);

            if (destination != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Insert the destination 
            Destination newDestination = new Destination
            {
                DestinationId = Guid.NewGuid().ToString(),
                Name = destinationName
            };

            //Add to the database
            db.Destinations.Add(newDestination);
            db.SaveChanges();

            return Json(new
            {
                message = $"لقد تم إضافة {destinationName} بنجاح",
                id = newDestination.DestinationId,
                name = destinationName
            }, JsonRequestBehavior.AllowGet);
        }

        // Get the detials of the destination 
        public ActionResult Details(string id)
        {
            // Try to find the id 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(400);

            // Check if there is an destination 
            var destination = db.Destinations.Find(id);

            if (destination == null)
                return new HttpStatusCodeResult(404);

            // Return the result 
            return Json(new
            {
                destination.Name,
                id = destination.DestinationId
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Destinations/Edit/3
        [HttpPost]
        public ActionResult Edit(string id, string newName)
        {
            // Check the data if it's valid 
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newName))
                return new HttpStatusCodeResult(406);

            // Check if there is an destination with this id 
            var destination = db.Destinations.Find(id);

            if (destination == null)
                return new HttpStatusCodeResult(404);

            // Check if there is another destination with the same name 
            newName = newName.Trim();

            var oldDestination = db.Destinations.SingleOrDefault(d => d.DestinationId != id && d.Name == newName);

            if (oldDestination != null)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);

            // Update the destination 
            destination.Name = newName;

            // Save the changes 
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

        // POST: Destinations/Delete/id
        [HttpPost]
        public ActionResult Delete(string id)
        {
            // Check if the id is existing 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Check if there is a destination with this id 
            var destination = db.Destinations.Find(id);

            if (destination == null)
                return new HttpStatusCodeResult(404);

            // Remove from the database 
            db.Destinations.Remove(destination);

            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }
    }
}