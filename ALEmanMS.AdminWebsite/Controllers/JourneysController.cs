using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class JourneysController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        #region HelperMethods

        // Function to get the drivers 
        SelectList GetDrivers()
        {
            var drivers = db.People.OfType<Driver>();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in drivers)
            {
                items.Add(new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.PersonId
                });
            }

            return new SelectList(items, "Value", "Text");
        }

        // Function to get the joureeyTypes  
        SelectList GetJourneyTypes()
        {
            var journeyTypes = db.JourneyTypes;

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in journeyTypes)
            {
                items.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.JourneyTypeId
                });
            }

            return new SelectList(items, "Value", "Text");
        }

        // Function to get the SenderComapneis  
        SelectList GetSenderCompanies()
        {
            var senderCompanies = db.SenderCompanies;

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in senderCompanies)
            {
                items.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.SenderCompanyId
                });
            }

            return new SelectList(items, "Value", "Text");
        }

        // Function to get the receiverComapneis  
        SelectList GetReceiverCompanies()
        {
            var receiverCompanies = db.ReceiverCompanies;

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in receiverCompanies)
            {
                items.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ReceiverCompanyId
                });
            }

            return new SelectList(items, "Value", "Text");
        }

        // Initialze the object
        JourneyViewModel initialzeViewModel(JourneyViewModel model = null)
        {
            if (model == null)
            {
                model = new JourneyViewModel();
                model.JourneyDate = DateTime.Now.Date;
            }


            model.SenderCompanies = GetSenderCompanies();
            model.ReceiverCompanies = GetReceiverCompanies();
            model.Drivers = GetDrivers();
            model.JourneyTypes = GetJourneyTypes();
            model.Categories = db.Categories.ToList();
            model.Customers = db.People.OfType<Customer>().ToList();
            model.Senders = db.People.OfType<Sender>().ToList();
            model.DriversList = db.People.OfType<Driver>().ToList();
            model.SenderCompaniesList = db.SenderCompanies.ToList();
            model.ReceiverCompaniesList = db.ReceiverCompanies.ToList();
            model.journeyTypesList = db.JourneyTypes.ToList();
            return model;
        }
        #endregion  

        public JourneysController()
        {
            ViewBag.Page = "Journeys";
            ViewBag.PageAr = "الرحلات";
        }


        // GET: Journeys
        public ActionResult Index()
        {
            return View(db.Journeys.ToList());
        }

        // GET: Journeys/Create
        public ActionResult Create()
        {
            return View(initialzeViewModel());
        }

        // POST: Journeys/Create
        [HttpPost]
        public ActionResult Create(JourneyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Location))
                    model.Location = model.Location.Trim();

                if (!string.IsNullOrEmpty(model.Policy))
                    model.Policy = model.Policy.Trim();

                // Get the data and validate it 
                var driver = db.People.Find(model.DriverId) as Driver; 
                
                if(driver == null)
                {
                    model.Message = "يوجد هنالك خطأ ما, يرجى إعادة المحاولة لاحقاً";
                    return View(initialzeViewModel(model)); 
                }

                var senderCompany = db.SenderCompanies.Find(model.SenderCompanyId); 
                if(senderCompany == null)
                {
                    model.Message = "يوجد هنالك خطأ ما, يرجى إعادة المحاولة لاحقاً";
                    return View(initialzeViewModel(model));
                }

                var receiverCompany = db.ReceiverCompanies.Find(model.ReceiverCompanyId); 
                if(receiverCompany == null)
                {
                    model.Message = "يوجد هنالك خطأ ما, يرجى إعادة المحاولة لاحقاً";
                    return View(initialzeViewModel(model));
                }

                var journeyType = db.JourneyTypes.Find(model.JourneyTypeId); 
                if(journeyType == null)
                {
                    model.Message = "يوجد هنالك خطأ ما, يرجى إعادة المحاولة لاحقاً";
                    return View(initialzeViewModel(model));
                }

                // Create new journey 
                var journey = new Journey
                {
                    JourneyId = Guid.NewGuid().ToString(), 
                    JourneyNumber = model.JourneyNumber,  
                    Customs = model.Customs,
                    DollarPrice = model.DollarPrice,
                    JourneyDate = model.JourneyDate,
                    Location = model.Location,
                    Policy = model.Policy,
                    Driver = driver,
                    JourneyType = journeyType,
                    SenderCompany = senderCompany,
                    ReceiverCompany = receiverCompany,
                    RyialPrice = model.RyialPrice
                };

                journey = db.Journeys.Add(journey);
                db.SaveChanges();

                return RedirectToAction("JourneyProfile", new { id = journey.JourneyId }); 
            }

            model.Message = "يوجد هنالك خطأ ما, يرجى التحقق من المدخلات";
            //initialzeViewModel(model);

            return View(initialzeViewModel(model));
        }

        // Delete the journey 
        // POST: Journeys/Delete/342
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            // Validate 
            if (id == null)
                return new HttpStatusCodeResult(406);

            var journey = db.Journeys.Find(id.Value);

            db.Journeys.Remove(journey);
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Open the journey Profile 
        public ActionResult JourneyProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            Journey journey = db.Journeys.Find(id);

            if (journey == null)
                return HttpNotFound();

            var model = initialzeViewModel();
            model.Journey = journey;
            model.Packages = db.Pakcages.Where(p => p.JourneyId == id).OrderBy(j => j.PackageNumber).ToList();

            return View(model); 
        }

        // Edit data of the journey 
        #region EditData
        // Edit the driver 
        // POST: Journeys/EditDrver
        [HttpPost]
        public ActionResult EditDriver(int id, string driverId)
        {
            // Validte
            if (string.IsNullOrEmpty(driverId))
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // get driver 
            var driver = db.People.Find(driverId) as Driver;
            if (driver == null)
                return HttpNotFound();

            journey.Driver = driver;

            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

        // Edit the sender Company 
        // POST: Journeys/EditSenderCompany
        [HttpPost]
        public ActionResult EditSenderCompany(int id, string senderCompanyId)
        {
            // Validte
            if (string.IsNullOrEmpty(senderCompanyId))
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // get senderCompany 
            var senderCompany = db.SenderCompanies.Find(senderCompanyId);
            if (senderCompany == null)
                return HttpNotFound();

            journey.SenderCompany = senderCompany;

            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit the receiver Company 
        // POST: Journeys/EditReceiverCompany
        [HttpPost]
        public ActionResult EditReceiverCompany(int id, string receiverCompanyId)
        {
            // Validte
            if (string.IsNullOrEmpty(receiverCompanyId))
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // get receiverCompany 
            var receiverCompany = db.ReceiverCompanies.Find(receiverCompanyId);
            if (receiverCompany == null)
                return HttpNotFound();

            journey.ReceiverCompany = receiverCompany;

            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit dollar Price 
        // POST: Journeys/EditDollarPrice 
        [HttpPost]
        public ActionResult EditDollarPrice(int id, decimal? dollarPrice)
        {
            // Validte
            if (dollarPrice == null)
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            journey.DollarPrice = dollarPrice.Value;
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit ryial Price 
        // POST: Journeys/EditRyialPrice 
        [HttpPost]
        public ActionResult EditRyialPrice(int id, decimal? ryialPrice)
        {
            // Validte
            if (ryialPrice == null)
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            journey.RyialPrice = ryialPrice.Value;
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit customs Price 
        // POST: Journeys/EditCustomsPrice 
        [HttpPost]
        public ActionResult EditCustomsPrice(int id, decimal? customsPrice)
        {
            // Validte
            if (customsPrice == null)
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            journey.Customs = customsPrice.Value;
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit customs Price 
        // POST: Journeys/EditCustomsPrice 
        [HttpPost]
        public ActionResult EditPolicy(int id, string policy)
        {
            // Validte
            if (string.IsNullOrEmpty(policy))
                return new HttpStatusCodeResult(406);

            // Get journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            journey.Policy = policy;
            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit Date 
        // POST: Journeys/EditDate 
        [HttpPost]
        public ActionResult EditDate(int id, string newDate)
        {
            if (string.IsNullOrEmpty(newDate))
                new HttpStatusCodeResult(406);

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            journey.JourneyDate = DateTime.Parse(newDate);

            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }

        // Edit JourneyType 
        // POST: Journeys/EditDate 
        [HttpPost]
        public ActionResult EditJourneyType(string id, string journeyTypeId)
        {
            if (string.IsNullOrEmpty(journeyTypeId) || string.IsNullOrEmpty(id))
                new HttpStatusCodeResult(406);

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            var type = db.JourneyTypes.Find(journeyTypeId);
            if (type == null)
            {
                return HttpNotFound();
            }

            journey.JourneyType = type;

            db.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // Edit Journey Number  
        // TODO: Ask Ziad to eidt the journey Number 
        // TODO: Implementing Edit journey Number 
        #endregion

        // Get statstics about each journey 
        public ActionResult Statistics(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Generate the statistics 
            List<StatisticItem> items = new List<StatisticItem>();
            List<StatisticItem> customerItems = new List<StatisticItem>();
            List<StatisticItem> senderItems = new List<StatisticItem>(); 

            var packagesByCategory = journey.Packages.GroupBy(p => p.Category);
            var packagesByCustomers = journey.Packages.GroupBy(p => p.Customer);
            var packagesBySenders = journey.Packages.GroupBy(p => p.Sender); 

            string[] colors =
            {
                "#00b19d",
                "#ef5350",
                "#3ddcf7",
                "#ffaa00",
                "#81c868",
                "#dcdcdc",
                "#3bafda"
            };
            int i = 0; 

            // Get the categories 
            foreach (var item in packagesByCategory)
            {
                StatisticItem newItem = new StatisticItem();
                // Calculate the result 
                // Check the status of the item 

                newItem.Value = (double)item.Key.Packages.Where(p => p.JourneyId == id).Sum(p => p.PackagesCount);
                newItem.Name = item.Key.Name;
                newItem.Color = colors[i];
                i++;

                items.Add(newItem); 
            }

            int i2 = 0; 

            // Get customers 
            foreach (var item in packagesByCustomers)
            {
                StatisticItem statistic = new StatisticItem();
                statistic.Color = colors[i2++];
                // Check the status of the item 
                statistic.Value = (double)item.Key.Packages.Where(p => p.JourneyId == id).Sum(p => p.PackagesCount);
                statistic.Name = item.Key.FullName;

                customerItems.Add(statistic); 
            }

            int i3 = 0; 
            // Get customers 
            foreach (var item in packagesBySenders)
            {
                StatisticItem statistic = new StatisticItem();
                statistic.Color = colors[i3++];
                // Check the status of the item 
                statistic.Value = (double)item.Key.Packages.Where(p => p.JourneyId == id).Sum(p => p.PackagesCount);
                statistic.Name = item.Key.FullName;

                senderItems.Add(statistic); 
            }

            return View(new StatisticViewModel
            {
                StatsticItemsCategories = items,
                StatsticItemsCustomers = customerItems,
                StatsticItemsSenders = senderItems
            }); 

        }
    }
}