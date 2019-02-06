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
            var model = new SenderViewModel
            {
                Cities = GetCities()
            };

            return View(model);
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
                    return new HttpStatusCodeResult(406);
                }

                var city = db.Cities.FirstOrDefault(c => c.Name == model.CityName);

                if (city == null)
                {
                    return new HttpStatusCodeResult(404);
                }
                var newSender = new Sender
                {
                    PersonId = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    Description = model.Description.Trim(),
                    Company = model.Company.Trim(),
                    MotherName = model.MotherName.Trim(),
                    NationalID = model.NationalId.Trim(),
                    Birthdate = model.BirthDate,
                    Profession = model.Profession.Trim(),
                    RegistrationNumber = model.RegisterationNumber.Trim(),
                    State = model.State.Trim(),
                    CityName = city.Name
                };

                db.People.Add(newSender);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(404);
        }

        //GET: Senders/Edit/fjsiday787ysd8afhsao
        public ActionResult Edit(string id)
        {
            var sender = db.People.Find(id) as Sender;

            if (sender == null)
            {
                return new HttpStatusCodeResult(404);
            }

            var model = new SenderViewModel
            {
                FirstName = sender.FirstName.Trim(),
                LastName = sender.LastName.Trim(),
                Description = sender.Description.Trim(),
                Company = sender.Company.Trim(),
                MotherName = sender.MotherName.Trim(),
                NationalId = sender.NationalID.Trim(),
                BirthDate = sender.Birthdate,
                Profession = sender.Profession.Trim(),
                RegisterationNumber = sender.RegistrationNumber.Trim(),
                State = sender.State.Trim(),
                CityName = sender.CityName,
                Cities = GetCities(),
            };

            return View(model);
        }

        //PUT: Senders/Edit
        [HttpPut]
        public ActionResult Edit(string id, SenderViewModel model)
        {
            if(ModelState.IsValid)
            {
                var oldSender = db.People.OfType<Sender>().FirstOrDefault(s => (s.FirstName == model.FirstName && s.LastName == model.LastName) || s.NationalID == model.NationalId);

                if (oldSender != null)
                {
                    return View(model);
                }

                var newSender = db.People.Find(id) as Sender;

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

            return new HttpStatusCodeResult(406);
        }
        [HttpDelete]
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