using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public enum EditType
    {
        category,
        sender,
        customer,
        count,
        packagesNumber,
        packagesCount,
        weight,
        planeWeight,
        casedNumber,
        originalCasedNumber,
        kilo, 
        dozen, 
        transfer,
        internalShipping
    }

    public class PackagesController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext(); 

        // GET: Packages
        public ActionResult Index()
        {
            return View();
        }

        //Method to add a new package
       [HttpPost]
        public ActionResult AddPackage(string id,PackageViewModel model)
        {
            // Validate the id 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            if (ModelState.IsValid)
            {
                // Get the journey 
                var journey = db.Journeys.Find(id);
                if (journey == null)
                    return HttpNotFound();

                // Get the sender 
                var sender = db.People.Find(model.SenderId) as Sender;
                if (sender == null)
                    return HttpNotFound();

                // Get the Customer
                var customer = db.People.Find(model.CustomerId) as Customer;
                if (customer == null)
                    return HttpNotFound();

                // Get the Customer
                var category = db.Categories.Find(model.CategoryId); 
                if (category == null)
                    return HttpNotFound();

                Package package = new Package
                {
                    CasedNumber = model.CasedNumber,
                    CasedOriginalNumber = model.CasedOriginalNumber,
                    Customer = customer,
                    Category = category,
                    Dozens = model.Dozen,
                    InternalShipmentPrice = model.InternalShipping,
                    Journey = journey,
                    Kilograms = model.Kilo,
                    PackageNumber = model.PackageNumber,
                    PackagingPrice = model.CasedNumber,
                    PackagesCount = model.PackageCounts,
                    PlaneWeight = model.PlaneWeight,
                    Quantity = model.Count,
                    Sender = sender,
                    Weight = model.Weight,
                    TransferPrice = model.Transfer,
                    PackageId = Guid.NewGuid().ToString()
                };

                db.Pakcages.Add(package);
                db.SaveChanges();

                return Json(new
                {
                    packageId = package.PackageId,
                    categoryId = category.CategoryId,
                    customerId = customer.PersonId,
                    senderId = sender.PersonId,
                    count = package.Quantity,
                    weight = package.Weight,
                    planeWeight = package.PlaneWeight, 
                    kilo = package.Kilograms,
                    dozen = package.Dozens,
                    casedNumber = package.CasedNumber,
                    originalCasedNumber = package.CasedOriginalNumber, 
                    transfer = package.TransferPrice, 
                    internalShipping = package.InternalShipmentPrice, 
                    packagesCount = package.PackagesCount, 
                    packageNumber = package.PackageNumber
                }, JsonRequestBehavior.AllowGet); 


            }

            // Failed 
            return new HttpStatusCodeResult(400); 
        }

        // Delete a package
        // POST: Packages/Delete/daf
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the package 
            var package = db.Pakcages.Find(id);
            if (package == null)
                return HttpNotFound();

            db.Pakcages.Remove(package);
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

        // Edit a package attribute 
        // POST: Packages/Delete/
        [HttpPost]
        public ActionResult Edit(string packageId, string type, string value)
        {
            // Validate the data 
            if (string.IsNullOrEmpty(packageId) || string.IsNullOrEmpty(type) || value == null)
                return new HttpStatusCodeResult(406);

            // Get the package 
            var package = db.Pakcages.Find(packageId);
            if (package == null)
                return HttpNotFound();

            EditType editType = (EditType)Enum.Parse(typeof(EditType), type);

            switch (editType)
            {
                case EditType.category:
                    string categoryId = value.ToString();
                    // Get the category 
                    var category = db.Categories.Find(categoryId);
                    if (category == null)
                        return HttpNotFound();

                    package.Category = category; 
                    break;
                case EditType.sender:
                    string senderId = value.ToString();
                    // Get the category 
                    var sender = db.People.Find(senderId) as Sender;
                    if (sender == null)
                        return HttpNotFound();

                    package.Sender = sender; 
                    break;
                case EditType.customer:
                    string customerId = value.ToString();
                    var customer = db.People.Find(customerId) as Customer;
                    if (customer == null)
                        return HttpNotFound();

                    package.Customer = customer; 
                    break;
                case EditType.count:
                    package.Quantity = int.Parse(value); 
                    break;
                case EditType.packagesNumber:
                    package.PackageNumber = int.Parse(value); 
                    break;
                case EditType.packagesCount:
                    package.PackagesCount = int.Parse(value); 
                    break;
                case EditType.weight:
                    package.Weight = double.Parse(value); 
                    break;
                case EditType.planeWeight:
                    package.PlaneWeight = double.Parse(value); 
                    break;
                case EditType.casedNumber:
                    package.CasedNumber = int.Parse(value); 
                    break;
                case EditType.originalCasedNumber:
                    package.CasedOriginalNumber = int.Parse(value); 
                    break;
                case EditType.kilo:
                    package.Kilograms = int.Parse(value); 
                    break;
                case EditType.dozen:
                    package.Dozens = int.Parse(value); 
                    break;
                case EditType.transfer:
                    package.TransferPrice = decimal.Parse(value); 
                    break;
                case EditType.internalShipping:
                    package.InternalShipmentPrice = decimal.Parse(value); 
                    break;
                default:
                    break;
            }

            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

        // ReNumber the packages 
        // GET: Packages/Numbering/54389dsnf
        public ActionResult Numbering(string id)
        {
            // Validate the id 
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            // Get the journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Get the packages 
            var packages = db.Pakcages.Where(p => p.JourneyId == id).OrderBy(p => p.PackageNumber);

            int counter = 1; 
            foreach (var item in packages)
            {
                item.PackageNumber = counter;
                counter += item.PackagesCount.Value; 
            }

            db.SaveChanges();

            return RedirectToAction("JourneyProfile", "Journeys", new { id = id }); 
           
        }
    }
}