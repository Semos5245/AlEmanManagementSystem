﻿using ALEmanMS.AdminWebsite.Models;
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
                    return View(model);

                var newCompany = new ReceiverCompany
                {
                    ReceiverCompanyId = Guid.NewGuid().ToString(),
                    Name = model.Name.Trim(),
                    Country = model.Country.Trim(),
                    CommercialNumber = model.CommercialNumber.Trim(),
                    Phone = model.Phone != null ? model.Phone.Trim() : "",
                    Description = model.Description != null ? model.Description.Trim() : "",
                };

                db.ReceiverCompanies.Add(newCompany);
                db.SaveChanges();

                return RedirectToAction("Index", "ReceiverCompanies");
            }

            return View(model);
        }

        //GET: ReceiverCompanies/Edit
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var oldreceiverCompany = db.ReceiverCompanies.Find(id);

            if (oldreceiverCompany == null)
                return HttpNotFound();

            var receiverCompany = new ReceiverCompanyViewModel
            {
                Name = oldreceiverCompany.Name,
                CommercialNumber = oldreceiverCompany.CommercialNumber,
                Country = oldreceiverCompany.Country,
                Phone = oldreceiverCompany.Phone,
                Description = oldreceiverCompany.Description
            };

            return View(receiverCompany);
        }

        //POST: ReceiverCompanies/Edit
        [HttpPost]
        public ActionResult Edit(string id, ReceiverCompanyViewModel model)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            if (ModelState.IsValid)
            {
                var company = db.ReceiverCompanies.Find(id);
                if (company == null)
                    return HttpNotFound();

                var oldCompany = db.ReceiverCompanies.SingleOrDefault(r => (r.Name == model.Name || r.CommercialNumber == model.CommercialNumber) && r.ReceiverCompanyId != id);
                if (oldCompany != null)
                    return View(model);

                company.Name = model.Name.Trim();
                company.Description = model.Description != null ? model.Description.Trim() : "";
                company.Country = model.Country.Trim();
                company.Phone = model.Phone != null ? model.Phone.Trim() : "";
                company.CommercialNumber = model.CommercialNumber.Trim();

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
                return new HttpStatusCodeResult(406);

            var company = db.ReceiverCompanies.Find(id);

            if (company == null)
                return HttpNotFound();

            db.ReceiverCompanies.Remove(company);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}