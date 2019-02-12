using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class SendersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public SendersController()
        {
            ViewBag.Page = "Senders"; 
        }

        SelectList GetCities()
        {
            var citiesList = new List<SelectListItem>();

            foreach (var item in db.Cities.ToList())
            {
                citiesList.Add(new SelectListItem { Text = item.Name, Value = item.CityId });
            }

            return new SelectList(citiesList, "Value", "Text");
        }

        // GET: Senders
        public ActionResult Index()
        {
            return View(db.People.OfType<Sender>().ToList());
        }

        //GET: Senders/Create
        public ActionResult Create()
        {
            return View(new SenderViewModel());
        }

        //POST: Senders/Create
        [HttpPost]
        public ActionResult Create(SenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sender = db.People.OfType<Sender>().FirstOrDefault(s => (s.FirstName == model.FirstName && s.LastName == model.LastName) || s.NationalID == model.NationalId);

                if (sender != null)
                {
                    return View(model);
                }

                //var city = db.Cities.FirstOrDefault(c => c.Name == model.CityId);

                //if (city == null)
                //{
                //    return HttpNotFound();
                //}

                var newSender = new Sender();
                newSender.PersonId = Guid.NewGuid().ToString();
                newSender.FirstName = model.FirstName.Trim();
                newSender.LastName = model.LastName.Trim();
                newSender.Description = model.Description != null ? model.Description.Trim() : "";
                newSender.Company = model.Company != null ? model.Company.Trim() : "";
                newSender.MotherName = model.MotherName != null ? model.MotherName.Trim() : "";
                newSender.NationalID = model.NationalId != null ? model.NationalId.Trim() : "";
                newSender.Birthdate = model.BirthDate;
                newSender.Profession = model.Profession != null ? model.Profession.Trim() : "";
                newSender.RegistrationNumber = model.RegisterationNumber != null ? model.RegisterationNumber.Trim() : "";
                newSender.State = model.State != null ? model.State.Trim() : "";
                newSender.CityName = model.CityName;

                db.People.Add(newSender);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //GET: Senders/Edit/fjsiday787ysd8afhsao
        public ActionResult Edit(string id)
        {
            var sender = db.People.Find(id) as Sender;

            if (sender == null)
            {
                return HttpNotFound();
            }

            var model = new SenderViewModel
            {
                FirstName = sender.FirstName,
                LastName = sender.LastName,
                Description = sender.Description,
                Company = sender.Company,
                MotherName = sender.MotherName,
                NationalId = sender.NationalID,
                BirthDate = sender.Birthdate,
                Profession = sender.Profession,
                RegisterationNumber = sender.RegistrationNumber,
                State = sender.State,
                CityName = sender.CityName,
                //Cities = GetCities(),
            };
            return View(model);
        }

        //PUT: Senders/Edit
        [HttpPost]
        public ActionResult Edit(string id, SenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldSender = db.People.OfType<Sender>().FirstOrDefault(s => (s.FirstName == model.FirstName && s.LastName == model.LastName) || s.NationalID == model.NationalId);

                if (oldSender != null)
                {
                    return View(model);
                }

                var newSender = db.People.Find(id) as Sender;

                if (newSender == null)
                    return HttpNotFound();

                // TODO: Fix the problem of null excpetion 

                newSender.FirstName = model.FirstName.Trim();
                newSender.LastName = model.LastName.Trim();
                newSender.MotherName = model.MotherName.Trim();
                newSender.NationalID = model.NationalId.Trim();
                newSender.Profession = model.Profession.Trim();
                newSender.RegistrationNumber = model.RegisterationNumber.Trim();
                newSender.State = model.State.Trim();
                newSender.Company = model.Company.Trim();
                newSender.Description = model.Description.Trim();
                newSender.Birthdate = model.BirthDate;
                newSender.CityName = model.CityName;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        //DELETE: Senders/Delete/juos7asg67iasg67isa
        public ActionResult Delete(string id)
        {
            var sender = db.People.Find(id) as Sender;

            if (sender == null)
            {
                return new HttpStatusCodeResult(406);
            }
            db.People.Remove(sender);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}