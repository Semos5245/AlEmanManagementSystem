using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class SenderCompaniesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: SenderCompanies
        public ActionResult Index()
        {
            return View(db.SenderCompanies.ToList());
        }

        //GET: SenderCompanies/Create
        public ActionResult Create()
        {
            return View(new SenderCompanyViewModel());
        }

        //POST: SenderCompanies/Create
        [HttpPost]
        public ActionResult Create(SenderCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldCompany = db.SenderCompanies.FirstOrDefault(r => r.Name == model.Name || r.CommercialNumber == model.CommercialNumber);

                if (oldCompany != null)
                {
                    return View(model);
                }

                var newCompany = new SenderCompany
                {
                    SenderCompanyId = Guid.NewGuid().ToString(),
                    Name = model.Name.Trim(),
                    Country = model.Country.Trim(),
                    CommercialNumber = model.CommercialNumber.Trim(),
                    Description = model.Description != null ? model.Description.Trim() : "",
                };

                db.SenderCompanies.Add(newCompany);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //GET: SenderCompanies/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var oldsenderCompany = db.SenderCompanies.Find(id);

            if (oldsenderCompany == null)
            {
                return HttpNotFound();
            }

            var senderCompany = new SenderCompanyViewModel
            {
                Name = oldsenderCompany.Name,
                CommercialNumber = oldsenderCompany.CommercialNumber,
                Country = oldsenderCompany.Country,
                Description = oldsenderCompany.Description
            };

            return View(senderCompany);
        }

        //POST: SenderCompanies/Edit
        [HttpPost]
        public ActionResult Edit(string id, SenderCompanyViewModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var oldCompany = db.SenderCompanies.Find(id);
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

        //POST: SenderCompanies/Delete
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var company = db.SenderCompanies.Find(id);

            if (company == null)
            {
                return HttpNotFound();
            }

            db.SenderCompanies.Remove(company);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}