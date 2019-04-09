using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class CustomsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext(); 

        // GET: Customs
        public ActionResult Index()
        {
            return View();
        }

        // method to generate the syrian customs 
        // GET: Customs/GenerateSyrianCustoms
        public ActionResult GenerateSyrianCustoms(string id)
        {
            // validate  
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(400);

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Generate the general info of the syrian journey custom 
            SyrianJourneyCustom custom = new SyrianJourneyCustom
            {
                JourneyCustomID = Guid.NewGuid().ToString(),
                JourneyDate = journey.JourneyDate,
                JourneyId = journey.JourneyId,
                Journey = journey,
                JourneyNumber = journey.JourneyNumber,
                ReceiverCompany = journey.ReceiverCompany.Name + " " + journey.ReceiverCompany.CommercialNumber,
                SenderCompany = journey.SenderCompany.Name + " " + journey.SenderCompany.CommercialNumber,
                SyrianCustomItems = new List<SyrianCustomItem>()
            };

            // Generate the tiems 
            var itemsByGroupingGroup = journey.Packages.GroupBy(p => p.Category.GroupingGroup);
            

            int total = 0;
            int totalDozens = 0;
            int totalPrice = 0;
            int totalWeight = 0;
            int totalNetWeight = 0;

            // Process each group of packages 
            foreach (var item in itemsByGroupingGroup)
            {
                // Get the sender 
                string senderName = "";

                // get the pakcages of this group 
                var senderPackages = item.GroupBy(p => p.Sender);
                if (senderPackages.Count() == 1)
                    senderName = senderPackages.FirstOrDefault().Key.FullName;

                // Get category price 
                decimal categoryPrice = item.Key.Price;

                // Get the cateogyr 
                string cateogryName = item.Key.Name;

                // Get the count 
                int count = 0;
                int dozens = 0; 
                // get the catgories of the group 
                var categories = item.GroupBy(p => p.Category);
                foreach (var category in categories)
                {
                    // Check the type of the category 
                    if (category.Key.Unit.Name.Contains("عدد") && item.Key.Unit.Name.Contains("عدد"))
                        count += (int)category.Sum(p => p.Quantity);
                    else
                        dozens += (int)category.Sum(p => p.Quantity + p.Kilograms + p.Dozens);
                }

                // Get the weight 
                int weight = (int)item.Sum(p => p.Weight);

                // Get the netweigt
                // TODO : Get the net weight 
                int netWeight = weight - 10;

                // Get the price
                int price = 14; // TODO: Get the price 

                // Get the total 
                int groupTotal = price * dozens;

                // get the packages cont 
                int packagesCount = (int)item.Sum(p => p.PackagesCount);

                SyrianCustomItem customItem = new SyrianCustomItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CategoryPrice = categoryPrice,
                    Cateogry = item.Key.Name,
                    Count = count,
                    Dozen = dozens,
                    SyrianJourneyCustomId = custom.JourneyCustomID,
                    SyrianJourneyCustom = custom
                };

                custom.SyrianCustomItems.Add(customItem); 
            }

            return View(); 
        }

        
    }
}