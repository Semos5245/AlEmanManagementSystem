using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class CustomerBillsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public CustomerBillsController()
        {
            ViewBag.Page = "Journeys";
            ViewBag.PageAr = "الرحلات";
        }

        // GET: CustomerBills
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(406);

            //var journey = db.Journeys.Find(id);

            //if (journey == null)
            //    return HttpNotFound();

            //// Check if there is bills for this journey 
            //var billsCount = journey.Bills.Count;
            //if (billsCount == 0)
            //    return HttpNotFound();

            var bill = db.Bills.Find(id.Value); 
            return View(bill);
        }

        // GET: CustomerBills/GoToBills
        public ActionResult GoToBills(string id)
        {
            // Validate input 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the journe y
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Get the bills 
            var billsCount = journey.Bills.Count;
            if (billsCount == 0)
                return HttpNotFound();

            // get the first bill id 
            int firstBillId = journey.Bills.FirstOrDefault().BillId;

            return RedirectToAction("Index", new { id = firstBillId }); 
        }

        // GET: Calculate the bills 
        // GET: CustomerBills/Generate/journeyId 
        
        public ActionResult Generate(string id)
        {
            // Check the param 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the journey 
            var journey = db.Journeys.Find(id);

            if (journey == null)
                return HttpNotFound();

            if (journey.IsBillGenerated)
                return RedirectToAction("GoToBills", new { id });

            // Get the packages 
            var packages = journey.Packages.ToList();

            // Get the customer of packages 
            var customers = packages.GroupBy(p => p.Customer);

            // Iterate over the customers 
            foreach (var customer in customers)
            {
                // Get the customer from the database 
                var dbCustomer = db.People.Find(customer.Key.PersonId) as Customer; 

                // Get the number of packages 1- 
                int packagesCount = (int)customer.Sum(p => p.PackagesCount);
                // Get the total weight -2 
                int totalWeight = (int)customer.Key.Packages.Sum(p => p.Weight.Value);
                // Get the price of the customer 

                // Get the settings 
                var setting = Setting.GetSettings(Server.MapPath("~/App_Data/Settings.json"));
                decimal price = customer.Key.SpecialFee; 
                if(price == 0)
                {
                    string journeyTypeName = journey.JourneyType.Name;
                    if (journeyTypeName.Contains("بري"))
                        price = setting.LandShippingPerKiloPrice; 
                    else if (journeyTypeName.Contains("بحري"))
                        price = setting.NauticalShippingPerKiloPrice;
                    else
                        price = setting.AirShippingPerKiloPrice;
                }
                // Get packaging price 
                decimal packagingPrice = setting.PackagingPrice;

                // Get packaging packages number 
                int packagesForPackaging = (int)customer.Key.Packages.Sum(p => p.CasedNumber);

                // Get the type of the packages 
                var groups = customer.Key.Packages.GroupBy(p => p.Category).GroupBy(c => c.Key.Group);
                int numberOfCategories = groups.Count();

                string packagesType = "مختلف"; 

                if(numberOfCategories == 1)
                    packagesType = groups.FirstOrDefault().Key.Name;

                // Get the senders 
                var senders = customer.Key.Packages.GroupBy(p => p.Sender);

                // Get the paying place 
                bool paidInDamas = customer.Key.PaidInDamas;

                /* Calculate the total 
                * 1- Calcualte number of packages 
                * 2- Calculate number of cased packages 
                * 3- Calculate internal shipping  
                * 4- Calcualte transfer 
                */

                // 1- Calcualte number of packages 
                decimal totalShippingCost = totalWeight * price;

                // 2- Calculate cased cost 
                decimal casedCost = packagingPrice * packagesForPackaging;

                // 3- Calcualte internal shipping 
                decimal internalShippingCost = customer.Key.Packages.Sum(p => p.InternalShipmentPrice.Value);

                // 4- Calculate transfer 
                decimal transferCost = customer.Key.Packages.Sum(p => p.TransferPrice.Value);

                decimal totalCost = totalShippingCost + casedCost + internalShippingCost + transferCost; 

                Bill bill = new Bill
                {
                    CarNumber = journey.Driver.VehicleNumber,
                    Customer = dbCustomer,
                    DriverName = journey.Driver.FullName,
                    Group = packagesType, 
                    Journey = journey, 
                    JourneyDate = journey.JourneyDate, 
                    PackageCount = packagesCount, 
                    PaidInDamas = paidInDamas, 
                    Weight= totalWeight, 
                    PlaneWeight = 0, 
                    ShipmentPrice = (double)price, 
                    Total = totalCost, 
                    TotalInSR = paidInDamas ? 0 : totalCost,
                    City = customer.Key.City.Name,
                    CustomerName = customer.Key.FullName + " - " + customer.Key.City.Name,
                    JourneyNumber = journey.JourneyNumber,
                    PackagingCount = packagesForPackaging
                };

                // Insert the bill items 
                bill.BillItems = new List<BillItem>();
                // Create a bill item for shipping cost 
                BillItem shippingBillItem = new BillItem
                {
                    Bill = bill,
                    Amount = totalShippingCost,
                    Description = "أجور الشحن",
                    BillItemId = Guid.NewGuid().ToString()
                };
                bill.BillItems.Add(shippingBillItem); 

                if(casedCost != 0)
                {
                    // Create a bill item for cased cost 
                    BillItem casedCostBillItem = new BillItem
                    {
                        Bill = bill,
                        Amount = casedCost,
                        Description = "أجور الخيش",
                        BillItemId = Guid.NewGuid().ToString()
                    };
                    bill.BillItems.Add(casedCostBillItem); 
                }
            

                // Create a bill item for internal shipping 
                if(internalShippingCost != 0)
                {
                    BillItem internalShippingBillItem  = new BillItem
                    {
                        Bill = bill,
                        Amount = internalShippingCost,
                        Description = "أجور الشحن الداخلي",
                        BillItemId = Guid.NewGuid().ToString()
                    };
                    bill.BillItems.Add(internalShippingBillItem); 
                }
              

                // Create a bill item for transfer 
                if(transferCost != 0)
                {

                    // Get the bill transfer for each sender 
                    var packagesWithTransfer = customer.Key.Packages.Where(p => p.TransferPrice != 0).ToList();

                    foreach (var item in packagesWithTransfer)
                    {
                        BillItem transferBillItem = new BillItem
                        {
                            Bill = bill,
                            Amount = item.TransferPrice.Value,
                            Description = "قيمة البضاعة | " + item.Sender.FullName,
                            BillItemId = Guid.NewGuid().ToString()
                        };
                        bill.BillItems.Add(transferBillItem); 
                    }
                }


                // Create a new list 
                bill.BillSenders = new List<BillSender>(); 

                // Add the senders to the bill 
                foreach (var sender in senders)
                {
                    // Create a bill sender 
                    BillSender billSender = new BillSender
                    {
                        Bill = bill,
                        BillSenderId = Guid.NewGuid().ToString(),
                        Description = sender.Key.FullName,
                        TotalPackages = (int)sender.Key.Packages.Sum(p => p.PackagesCount)
                    };

                    bill.BillSenders.Add(billSender); 
                    
                }

                // Add the bill to the database 
                journey.IsBillGenerated = true; 
                db.Bills.Add(bill);
                db.SaveChanges(); 

            }
            return RedirectToAction("GoToBills", new { id = journey.JourneyId });
        }

        // Edit Bill Info 
        // POST: CustomerBills/EditBillInfo/435
        [HttpPost]
        public ActionResult EditBillInfo(int? id, BillInfoViewModel model)
        {
            // Validate 
            if (id == null)
                return new HttpStatusCodeResult(406);

            var bill = db.Bills.Find(id.Value);
            if (bill == null)
                return HttpNotFound();

            // Validate the model 
            if (ModelState.IsValid)
            {
                // Update the bill info 
                bill.CarNumber = model.CarNumber.Trim();
                bill.City = model.City.Trim();
                bill.CustomerName = model.CustomerName.Trim();
                bill.DriverName = model.DriverName.Trim();
                bill.Group = model.Group;
                bill.JourneyNumber = model.JourneNumber;
                bill.JourneyDate = model.JourneyDate;
                bill.Notes = model.Notes;
                bill.Total = model.Total;
                bill.TotalInSR = model.TotalInSR;
                bill.Weight = model.Weight;
                bill.PlaneWeight = model.PlaneWeight;
                bill.PackageCount = model.PackagingCount;
                bill.PackageCount = model.PackageCount;

                db.SaveChanges();

                updateBillPrice(id.Value);
                
                // TODO: Update the bill depends on the total adn weight not only bill Items
                 

                return RedirectToAction("Index", new { id = id.Value });
            }

            return RedirectToAction("Index", new { id = id.Value }); 
        }
        #region BillItems 
        // Inserting a new bill item 
        // POST: CustomerBills/InsertBillItem/54
        [HttpPost]
        public ActionResult InsertBillItem(int? id, string description, decimal? amount)
        {
            // Validate the input 
            if (id == null || string.IsNullOrEmpty(description) || amount == null)
                return new HttpStatusCodeResult(406);

            // Get hte bill 
            Bill bill = db.Bills.Find(id.Value);
            if (bill == null)
                return HttpNotFound();

            // Insert a new bill item into the bill 
            if (bill.BillItems == null)
                bill.BillItems = new List<BillItem>();

            // Create a new bill item 
            BillItem billItem = new BillItem
            {
                BillItemId = Guid.NewGuid().ToString(),
                Bill = bill,
                Amount = amount.Value,
                Description = description
            };

            db.BillItems.Add(billItem);
            db.SaveChanges();

            var total = updateBillPrice(id.Value);

            return Json(new
            {
                billItemId = billItem.BillItemId, 
                total,
                description = billItem.Description, 
                amount = billItem.Amount
            }, JsonRequestBehavior.AllowGet); 
        }

        // Get the details of the bill item 
        public ActionResult BillItemDetails(string id)
        {
            // Validate the id 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the bill item 
            var billItem = db.BillItems.Find(id);
            if (billItem == null)
                return HttpNotFound();

            return Json(new
            {
                description = billItem.Description, 
                amount = billItem.Amount, 
                billItemId = id
            }, JsonRequestBehavior.AllowGet); 
        }

        // Edit a bill item 
        // POST: CustomerBills/EditBillItem/435 
        [HttpPost]
        public ActionResult EditBillItem(string id, string newDescription, decimal? newAmount)
        {
            // Validate input 
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newDescription) || newAmount == null)
                return new HttpStatusCodeResult(406);

            // Get the bill item 
            var billItem = db.BillItems.Find(id);
            if (billItem == null)
                return HttpNotFound();

            billItem.Description = newDescription.Trim();
            billItem.Amount = newAmount.Value;

            db.SaveChanges();

            var total = updateBillPrice(billItem.Bill.BillId);

            return Json(new
            {
                description = newDescription.Trim(), 
                amount = newAmount.Value, 
                billItemId = id, 
                total
            }, JsonRequestBehavior.AllowGet); 
        }


        // Method to remove a bill item 
        [HttpPost]
        public ActionResult DeleteBillItem(string id)
        {
            // validate 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // get the bill 
            var billItem = db.BillItems.Find(id);
            if (billItem == null)
                return HttpNotFound();

            db.BillItems.Remove(billItem);
            db.SaveChanges();

            var total = updateBillPrice(billItem.Bill.BillId);

            return Json(new
            {
                total
            }, JsonRequestBehavior.AllowGet); 
        }

        #endregion

        // Deal with senders of the bill 
        #region BillSenders 
        // Insert a new bill sender 
        // PSOT: CustomerBills/InsertBillSender/343
        [HttpPost]
        public ActionResult InsertBillSender(int? id, string description, int? packages)
        {
            // VAldiadte the input 
            if (id == null || string.IsNullOrEmpty(description) || packages == null)
                return new HttpStatusCodeResult(406);

            // Get the bill id 
            var bill = db.Bills.Find(id.Value);
            if (bill == null)
                return HttpNotFound();

            // Create the sender object 
            if (bill.BillSenders == null)
                bill.BillSenders = new List<BillSender>();

            BillSender sender = new BillSender
            {
                BillSenderId = Guid.NewGuid().ToString(),
                Description = description.Trim(),
                TotalPackages = packages.Value,
                Bill = bill
            };

            db.BillSenders.Add(sender);
            db.SaveChanges();

            return Json(new
            {
                // Return the id 
                billSenderId = sender.BillSenderId,
                description, 
                packages
            }, JsonRequestBehavior.AllowGet); 
        }

        // Get the details of the bill sender 
        // GET: CustomerBills/GetBillSenderDetails/34
        public ActionResult GetBillSenderDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // get the bill sender 
            var billSender = db.BillSenders.Find(id);
            if (billSender == null)
                return HttpNotFound();

            return Json(new
            {
                billSenderId = billSender.BillSenderId,
                description = billSender.Description,
                packages = billSender.TotalPackages
            }, JsonRequestBehavior.AllowGet);
        }

        // Edit the bill sender 
        // POST: CustomerBills/EditBillSender/3432efeg
        [HttpPost]
        public ActionResult EditBillSender(string id, string newDescription, int? newPackages)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(newDescription) || newPackages == null)
                return new HttpStatusCodeResult(406);

            var billSender = db.BillSenders.Find(id);
            if (billSender == null)
                return HttpNotFound();

            billSender.Description = newDescription.Trim();
            billSender.TotalPackages = newPackages.Value;

            db.SaveChanges();

            return Json(new
            {
                id, 
                newDescription,
                newPackages.Value
            }, JsonRequestBehavior.AllowGet); 
        }

        // Delete bill sneder 
        // POST: CustomerBills/DeletebillSender/34efgsd 
        [HttpPost]
        public ActionResult DeleteBillSender(string id)
        {
            // validate 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var billSender = db.BillSenders.Find(id);
            if (billSender == null)
                return HttpNotFound();

            db.BillSenders.Remove(billSender);
            db.SaveChanges();

            return new HttpStatusCodeResult(200); 
        }
        #endregion 
        #region HelperMethods
        // Method to update the total price of the bill after adding items  
        decimal updateBillPrice(int billId)
        {
            // GEt the bill 
            var bill = db.Bills.Find(billId);

            // recalcuate the bill 
            
            decimal total = 0; 

            // Calculate the bill items 
            foreach (var item in bill.BillItems)
            {
                total += item.Amount; 
            }

            bill.Total = total;

            if (bill.PaidInDamas)
                bill.TotalInSR = 0;
            else
                bill.TotalInSR = total;

            db.SaveChanges(); 

            return total; 
        }
        #endregion 
    }
}
