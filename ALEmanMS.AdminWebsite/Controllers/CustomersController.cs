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
            if (ModelState.IsValid)
            {
                var customer = db.People.OfType<Customer>().FirstOrDefault(c => (c.FirstName == model.FirstName && c.LastName == model.LastName) || c.CommercialName == model.CommercialName);

                if (customer != null)
                    return View(model);

                var city = db.Cities.FirstOrDefault(c => c.Name == model.CityName);

                if (city == null)
                    return View(model);

                var newCustomer = new Customer
                {
                    PersonId = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    Company = model.Company.Trim(),
                    CommercialName = model.CommercialName.Trim(),
                    PaidInDamas = model.PaidInDamascus == "on" ? true: false,
                    SpecialFee = model.SpecialFee,
                    Description = model.Description.Trim(),
                    Birthdate = model.BirthDate,
                    TaxNumber = model.TaxtNumber.Trim(),
                    City = city,
                    CityId = city.CityId,
                    CityName = model.CityName.Trim()
                };

                db.People.Add(newCustomer);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Get: Customers/Edit/fjusadyf6asgfsadg6f7asfi
        public ActionResult Edit(string id)
        {
            var customer = db.People.Find(id) as Customer;

            if (customer == null)
                return new HttpStatusCodeResult(406);

            var model = new CustomerViewModel
            {
                FirstName = customer.FirstName.Trim(),
                LastName = customer.LastName.Trim(),
                Company = customer.Company.Trim(),
                CommercialName = customer.CommercialName.Trim(),
                BirthDate = customer.Birthdate,
                TaxtNumber = customer.TaxNumber.Trim(),
                Description = customer.Description.Trim(),
                Cities = GetCities(),
                SpecialFee = customer.SpecialFee,
                CityName = customer.CityName.Trim()
            };

            return View(model);
        }

        //PUT: Customer/Edit/fjusadyf6asgfsadg6f7asfi
        [HttpPut]
        public ActionResult Edit(string id, CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldCustomer = db.People.OfType<Customer>().FirstOrDefault(c => (c.FirstName == model.FirstName && c.LastName == model.LastName) || c.CommercialName == model.CommercialName);

                if (oldCustomer != null)
                    return View(model);

                var city = db.Cities.FirstOrDefault(c => c.Name == model.CityName);
                var newCustomer = db.People.Find(id) as Customer;

                newCustomer.FirstName = model.FirstName.Trim();
                newCustomer.LastName = model.LastName.Trim();
                newCustomer.PaidInDamas = model.PaidInDamascus == "on" ? true : false;
                newCustomer.SpecialFee = model.SpecialFee;
                newCustomer.TaxNumber = model.TaxtNumber.Trim();
                newCustomer.Description = model.Description.Trim();
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
        [HttpDelete]
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