using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class CategoriesController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(new CategoryViewModel
            {
                Categories = db.Categories.ToList(),
                Units = db.Units.ToList(),
                GroupingGroups = db.GroupingGroups.ToList(),
                Groups = db.Groups.ToList()
            });
        }

        // Insert a new category 
        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(string name, string unitId, string groupId, string groupingGroupId, string lowGroupingGroupId)
        {
            // Validate the fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(unitId) || string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(groupingGroupId) || string.IsNullOrEmpty(lowGroupingGroupId))
                return new HttpStatusCodeResult(406);

            name = name.Trim();
            unitId = unitId.Trim();
            groupId = groupId.Trim();
            groupingGroupId = groupingGroupId.Trim();
            lowGroupingGroupId = lowGroupingGroupId.Trim();

            // Get the items and check if they are existing  
            var unit = db.Units.Find(unitId);
            if (unit == null)
                return HttpNotFound();

            var group = db.Groups.Find(groupId);
            if (group == null)
                return HttpNotFound();

            var groupingGroup = db.GroupingGroups.Find(groupingGroupId);
            if (groupingGroup == null)
                return HttpNotFound();

            var lowGroupingGroup = db.GroupingGroups.Find(lowGroupingGroupId);
            if (lowGroupingGroup == null)
                return HttpNotFound();

            var oldCategory = db.Categories.SingleOrDefault(c => c.Name == name);
            if (oldCategory != null)
                return new HttpStatusCodeResult(409);

            // Add the category 
            var category = new Category
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = name,
                Group = group,
                GroupingGroup = groupingGroup,
                LowGroupingGroup = lowGroupingGroup,
                Unit = unit
            };

            db.Categories.Add(category);
            db.SaveChanges();

            return Json(new
            {
                id = category.CategoryId,
                name = category.Name,
                unit = category.Unit.Name,
                groupingGroup = category.GroupingGroup.Name,
                group = category.Group.Name,
                lowGroupingGroup = category.LowGroupingGroup.Name
            }, JsonRequestBehavior.AllowGet);
        }

        // Get the details of each cateogry 
        // GET: Categories/Details/dsfa 
        public ActionResult Details(string id)
        {
            // validate 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            id = id.Trim();
            var category = db.Categories.Find(id);

            return Json(new
            {
                id = id,
                name = category.Name,
                unitId = category.UnitId,
                groupId = category.GroupId,
                groupingGroupId = category.GroupingGroupId,
                lowGroupingGroup = category.LowGroupingGroupId
            }, JsonRequestBehavior.AllowGet);
        }

        // Edit the category 
        [HttpPost]
        public ActionResult Edit(string id, string newName, string unitId, string groupId, string groupingGroupId, string lowGroupingGroupId)
        {
            // Valdite 
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(unitId)
                || string.IsNullOrEmpty(groupId) || string.IsNullOrEmpty(groupingGroupId) || string.IsNullOrEmpty(lowGroupingGroupId))
                return new HttpStatusCodeResult(406);

            id = id.Trim();
            newName = newName.Trim();
            unitId = unitId.Trim();
            groupId = groupId.Trim();
            groupingGroupId = groupingGroupId.Trim();
            lowGroupingGroupId = lowGroupingGroupId.Trim();

            // Get the category  
            var category = db.Categories.Find(id);
            if (category == null)
                return HttpNotFound();

            // Get the data 
            var unit = db.Units.Find(unitId);
            if (unit == null)
                return HttpNotFound();

            var group = db.Groups.Find(groupId);
            if (group == null)
                return HttpNotFound();

            var groupingGroup = db.GroupingGroups.Find(groupingGroupId);
            if (groupingGroup == null)
                return HttpNotFound();

            var lowGroupingGroup = db.GroupingGroups.Find(lowGroupingGroupId);
            if (lowGroupingGroupId == null)
                return HttpNotFound();

            // Check if there is another cateogry with the same name 
            var oldCateogry = db.Categories.SingleOrDefault(c => c.CategoryId != id && c.Name == newName);
            if (oldCateogry != null)
                return new HttpStatusCodeResult(409);

            // Edit the data 
            category.Name = newName;
            category.Unit = unit;
            category.LowGroupingGroup = lowGroupingGroup;
            category.GroupingGroup = groupingGroup;
            category.Group = group;

            db.SaveChanges();

            return Json(new
            {
                name = category.Name,
                groupingGroup = groupingGroup.Name,
                lowGroupingGroup = lowGroupingGroup.Name,
                unit = unit.Name,
                group = group.Name,
                id
            }, JsonRequestBehavior.AllowGet); 
        }

        // Delete a category 
        [HttpPost]
        public ActionResult Delete(string id)
        {
            // Vcalitge 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var category = db.Categories.Find(id);
            if (category == null)
                return HttpNotFound();

            db.Categories.Remove(category);
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

    }
}