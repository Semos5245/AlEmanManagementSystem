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
    public class GroupingGroupsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public GroupingGroupsController()
        {
            ViewBag.Page = "GroupingGroups";
        }

        // GET: GroupingGroups
        public ActionResult Index()
        {
            return View(new GroupingGroupsViewModel
            {
                GroupingGroups = db.GroupingGroups.ToList(),
                Units = db.Units.ToList()
            });
        }

        // POST: GroupingGroups/Create
        [HttpPost]
        public ActionResult Create(string name, decimal? price, string unitId)
        {
            // VAlidate the fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(unitId) || price == null)
                return new HttpStatusCodeResult(406);

            unitId = unitId.Trim();
            name = name.Trim(); 

            // Get the unit 
            var unit = db.Units.Find(unitId);

            if (unit == null)
                return new HttpStatusCodeResult(404);

            // Check if there is another group
            var oldGroupingGroup = db.GroupingGroups.SingleOrDefault(g => g.Name == name);

            if (oldGroupingGroup != null)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict); 

            GroupingGroup groupingGroup = new GroupingGroup
            {
                GroupingGroupId = Guid.NewGuid().ToString(),
                Name = name,
                Price = price.Value,
                Unit = unit
            };

            db.GroupingGroups.Add(groupingGroup);
            db.SaveChanges();

            return Json(new
            {
                name = groupingGroup.Name,
                id = groupingGroup.GroupingGroupId,
                unit = groupingGroup.Unit.Name,
                price = groupingGroup.Price, 
            }, JsonRequestBehavior.AllowGet); 
        }

        // Get the details 
        // GET: GroupingGroups/Detials/dsafl
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            id = id.Trim();

            var groupingGroup = db.GroupingGroups.Find(id);

            if (groupingGroup == null)
                return new HttpNotFoundResult();

            return Json(new
            {
                name = groupingGroup.Name, 
                price = groupingGroup.Price,
                unit = groupingGroup.Unit.Name,
                unitId = groupingGroup.Unitid,
                id = groupingGroup.GroupingGroupId
            }, JsonRequestBehavior.AllowGet); 
        }

        // Edit the group 
        // POST: GroupingGroup/Edit/dsfa;
        [HttpPost]
        public ActionResult Edit(string id, string newName, decimal? price, string unitId)
        {
            // Valdiate the input 
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(unitId) || price == null)
                return new HttpStatusCodeResult(406);

            id = id.Trim();
            newName = newName.Trim();
            unitId = unitId.Trim();

            var groupingGroup = db.GroupingGroups.Find(id);

            if (groupingGroup == null)
                return HttpNotFound();

            // Get the unit 
            var unit = db.Units.Find(unitId);

            if (unit == null)
                return HttpNotFound();

            // Check if there is another groupingGroup with the same name 
            var oldGroupingGroup = db.GroupingGroups.SingleOrDefault(g => g.GroupingGroupId != id && g.Name == newName);

            if (oldGroupingGroup != null)
                return new HttpStatusCodeResult(409);

            groupingGroup.Name = newName;
            groupingGroup.Price = price.Value;
            groupingGroup.Unit = unit;

            db.SaveChanges();

            return Json(new
            {
                name = newName, 
                price = price.Value, 
                unit = unit.Name, 
                unitId = unitId
            }, JsonRequestBehavior.AllowGet);
        }

        // Delete Grouping Group 
        [HttpPost]
        public ActionResult Delete(string id)
        {
            // Validatet 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var groupingGroup = db.GroupingGroups.Find(id);

            if (groupingGroup == null)
                return HttpNotFound();

            db.GroupingGroups.Remove(groupingGroup);
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }
    }
}