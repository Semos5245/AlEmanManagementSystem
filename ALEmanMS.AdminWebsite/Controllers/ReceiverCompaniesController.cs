using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class ReceiverCompaniesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ReceiverCompaniesController()
        {
            ViewBag.Page = "ReceiverCompanies";
        }

        // GET: ReceiverCompanies
        public ActionResult Index()
        {
            return View(db.ReceiverCompanies.ToList());
        }

        //GET: ReceiverCompanies/Create
        public ActionResult Create()
        {
            return View(new ReceiverCompanyViewModel());
        }

        //POST: ReceiverCompanies/Create
        [HttpPost]
        public ActionResult Create(ReceiverCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldCompany = db.ReceiverCompanies.FirstOrDefault(r => r.Name == model.Name || r.CommercialNumber == model.CommercialNumber);

                if (oldCompany != null)
                {
                    return View(model);
                }

                var newCompany = new ReceiverCompany
                {
                    ReceiverCompanyId = Guid.NewGuid().ToString(),
                    Name = model.Name.Trim(),
                    Country = model.Country.Trim(),
                    CommercialNumber = model.CommercialNumber.Trim(),
                    Description = model.Description != null ? model.Description.Trim() : "",
                };

                db.ReceiverCompanies.Add(newCompany);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //GET: ReceiverCompanies/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var oldreceiverCompany = db.ReceiverCompanies.Find(id);

            if (oldreceiverCompany == null)
            {
                return HttpNotFound();
            }

            var receiverCompany = new ReceiverCompanyViewModel
            {
                Name = oldreceiverCompany.Name,
                CommercialNumber = oldreceiverCompany.CommercialNumber,
                Country = oldreceiverCompany.Country,
                Description = oldreceiverCompany.Description
            };

            return View(receiverCompany);
        }

        //POST: ReceiverCompanies/Edit
        [HttpPost]
        public ActionResult Edit(string id, ReceiverCompanyViewModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var oldCompany = db.ReceiverCompanies.Find(id);
                if (oldCompany == null)
                {
                    return HttpNotFound();
                }
                
                oldCompany.Name = model.Name.Trim();
                oldCompany.Description = model.Description != null ? model.Description.Trim() : "";
                oldCompany.Country = model.Country.Trim();
                oldCompany.CommercialNumber = model.CommercialNumber.Trim();

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //POST: ReceiverCompanies/Delete
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var company = db.ReceiverCompanies.Find(id);

            if (company == null)
            {
                return HttpNotFound();
            }

            db.ReceiverCompanies.Remove(company);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}