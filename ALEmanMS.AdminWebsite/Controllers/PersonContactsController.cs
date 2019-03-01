using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class PersonContactsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public PersonContactsController()
        {
            this.ViewBag.Page = "PersonContacts";
            this.ViewBag.PageAr = "الإتصالات";
        }

        // GET: PersonContacts
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(409);

            var oldPerson = db.People.Find(id);
            if (oldPerson == null)
                return HttpNotFound();

            var model = new PersonContactViewModel
            {
                Person = oldPerson,
                PersonContacts = oldPerson.PersonContacts
            };
            
            return View(model);
        }

        //POST: PersonContacts/nrfuh278
        [HttpPost]
        public ActionResult AddContact(string id, PersonContactViewModel model)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var person = db.People.Find(id);
            if (person == null)
                return HttpNotFound();

            model.PersonContacts = person.PersonContacts;
            model.Person = person;

            if (ModelState.IsValid)
            {
                var contact = db.PersonContacts.SingleOrDefault(c => (c.Value == model.Value && c.PersonId == id));
                if (contact != null)
                    return new HttpStatusCodeResult(406);

                var _newContact = new Contact { ContactId = Guid.NewGuid().ToString(), Name = model.ContactName };

                var newContact = new PersonContact
                {
                    PersonContactId = Guid.NewGuid().ToString(),
                    Person = person,
                    Contact = _newContact,
                    ContactId = _newContact.ContactId,
                    Value = model.Value,
                    PersonId = person.PersonId
                };

                db.PersonContacts.Add(newContact);
                db.SaveChanges();

                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(406);
        }

        //DELETE: PersonContact/fdshfas78
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var personContact = db.PersonContacts.Find(id);

            if (personContact == null)
                return HttpNotFound();

            db.PersonContacts.Remove(personContact);
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }
    }
}