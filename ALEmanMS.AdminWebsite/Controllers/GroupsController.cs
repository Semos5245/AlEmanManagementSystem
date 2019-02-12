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
    public class GroupsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public GroupsController()
        {
            ViewBag.Page = "Groups";
        }

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }


        // GET: Groups/Details/sdlf
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var group = db.Groups.Find(id);

            if (group == null)
                return new HttpStatusCodeResult(404);

            return Json(new
            {
                id = group.GroupId,
                name = group.Name
            }, JsonRequestBehavior.AllowGet);
        }
        // POST: Groups/Create
        [HttpPost]
        public ActionResult Create(string name)
        {
            // Validate the name 
            if (string.IsNullOrEmpty(name))
                return new HttpStatusCodeResult(406);

            name = name.Trim();

            // Check if there is another group with the same name 
            var group = db.Groups.SingleOrDefault(g => g.Name == name);

            if (group != null)
                return new HttpStatusCodeResult(409);

            // Add the group 
            Group newGroup = new Group
            {
                Name = name,
                GroupId = Guid.NewGuid().ToString()
            };

            db.Groups.Add(newGroup);
            db.SaveChanges();

            return Json(new
            {
                id = newGroup.GroupId,
                name = newGroup.Name
            },JsonRequestBehavior.AllowGet); 
        }

        // Edit Groups/Create 
        [HttpPost]
        public ActionResult Edit(string id, string newName)
        {
            // Validate 
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newName))
                return new HttpStatusCodeResult(406);

            id = id.Trim();
            newName = newName.Trim(); 

            var group = db.Groups.Find(id);

            if (group == null)
                return new HttpStatusCodeResult(404);



            // Check if there is a groupo with the same new name 
            var oldGroup = db.Groups.SingleOrDefault(g => g.GroupId != id && g.Name == newName);

            if (oldGroup != null)
                return new HttpStatusCodeResult(409);

            group.Name = newName;

            db.SaveChanges();

            return Json(new
            {
                id = group.GroupId,
                name = group.Name
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Groups/Delete/afsd
        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the item 
            var group = db.Groups.Find(id);

            if (group == null)
                return new HttpStatusCodeResult(404);

            db.Groups.Remove(group);
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

    }
}