using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class CustomersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public CustomersController()
        {
            ViewBag.Page = "Customers";
        }

        //Get Cities
        SelectList GetCities()
        {
            var citiesSelectList = new List<SelectListItem>();

            foreach (var item in db.Cities.ToList())
            {
                citiesSelectList.Add(new SelectListItem { Text = item.Name, Value = item.CityId.ToString() });
            }

            return new SelectList(citiesSelectList, "Value", "Text");
        }

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.People.OfType<Customer>().ToList());
        }

        //GET: Customers/Create
        public ActionResult Create()
        {
            var model = new CustomerViewModel
            {
                Cities = GetCities()
            };
            return View(model);
        }

        //POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerViewModel model)
        {
            model.Cities = GetCities();

            if (ModelState.IsValid)
            {
                var customer = db.People.OfType<Customer>().FirstOrDefault(c => (c.FirstName == model.FirstName && c.LastName == model.LastName) || c.CommercialName == model.CommercialName);

                if (customer != null)
                    return View(model);

                var city = db.Cities.Find(model.CityId);

                if (city == null)
                    return View(model);

                var newCustomer = new Customer
                {
                    PersonId = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    Company = model.Company != null ? model.Company.Trim() : "",
                    CommercialName = model.CommercialName.Trim(),
                    PaidInDamas = model.PaidInDamascus == "on" ? true: false,
                    SpecialFee = model.SpecialFee,
                    Description = model.Description != null ? model.Description.Trim() : "",
                    Birthdate = model.BirthDate,
                    TaxNumber = model.TaxtNumber != null ? model.TaxtNumber.Trim() : "",
                    City = city,
                    CityId = city.CityId,
                    CityName = city.Name
                };

                db.People.Add(newCustomer);
                db.SaveChanges();

                return RedirectToAction("Index","PersonContacts",new { id = newCustomer.PersonId });
            }
            return View(model);
        }

        //Get: Customers/Edit/fjusadyf6asgfsadg6f7asfi
        public ActionResult Edit(string id)
        {
            var customer = db.People.Find(id) as Customer;

            if (customer == null)
                return HttpNotFound();

            var model = new CustomerViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Company = customer.Company,
                CommercialName = customer.CommercialName,
                BirthDate = customer.Birthdate,
                TaxtNumber = customer.TaxNumber,
                Description = customer.Description,
                Cities = GetCities(),
                SpecialFee = customer.SpecialFee,
                CityName = customer.CityName,
                PaidInDamascus = customer.PaidInDamas == true ? "on" : "off"
            };

            return View(model);
        }

        //PUT: Customer/Edit/fjusadyf6asgfsadg6f7asfi
        [HttpPost]
        public ActionResult Edit(string id, CustomerViewModel model)
        {
            model.Cities = GetCities();

            if (ModelState.IsValid)
            {
                //var oldCustomer = db.People.OfType<Customer>().FirstOrDefault(c => (c.FirstName == model.FirstName && c.LastName == model.LastName) || c.CommercialName == model.CommercialName);

                //if (oldCustomer == null)
                    //return View(model);

                var city = db.Cities.Find(model.CityId);
                var newCustomer = db.People.Find(id) as Customer;

                if (city == null || newCustomer == null)
                {
                    return View(model);
                }

                newCustomer.FirstName = model.FirstName.Trim();
                newCustomer.LastName = model.LastName.Trim();
                newCustomer.PaidInDamas = model.PaidInDamascus == "on" ? true : false;
                newCustomer.SpecialFee = model.SpecialFee;
                newCustomer.TaxNumber = model.TaxtNumber != null ? model.TaxtNumber.Trim() : "";
                newCustomer.Description = model.Description != null ? model.Description.Trim() : "";
                newCustomer.CommercialName = model.CommercialName.Trim();
                newCustomer.Birthdate = model.BirthDate;
                newCustomer.City = city;
                newCustomer.CityId = city.CityId;
                newCustomer.CityName = city.Name;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //DELETE: Customer/Delete/fjsu8adhf7safsaf67
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var customer = db.People.Find(id) as Customer;

            if (customer == null)
                return new HttpStatusCodeResult(404);

            db.People.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}