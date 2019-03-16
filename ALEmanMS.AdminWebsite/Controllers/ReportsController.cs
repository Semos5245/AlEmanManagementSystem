using ALEmanMS.AdminWebsite.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ALEmanMS.AdminWebsite.Datasets.ReportsDataset;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class ReportsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext(); 

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        // CalculationsTable Report 
        public ActionResult CalculationsTable(string id)
        {
            // Get the journey 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Get the packages depends on the customer 
            var packagesByCustomers = journey.Packages.GroupBy(p => p.Customer).OrderBy(p => p.Key.City.CityOrder);

            DataSet ds = new DataSet(); 

            // Create the datasource 
            CalculationsTableDataTable dt = new CalculationsTableDataTable();
            ds.Tables.Add(dt); 
            int totalPackages = 0;
            int totalWeight = 0; 

            // loop over the items 
            foreach (var item in packagesByCustomers)
            {
                // Get the type of the packages 
                var packagesOfCustomer = item.Key.Packages.GroupBy(p => p.Category.Group);
                foreach (var customerByGroup in packagesOfCustomer)
                {
                    // Calculate the transfers 
                    var categories = customerByGroup.Key.Categories;
                    decimal transfers = 0; 
                    decimal internalShipping = 0;
                    decimal casing = 0;
                    int packages = 0;
                    int weight = 0; 
                    string sender = ""; 
                    foreach (var category in categories)
                    {
                        // Get the sender name 
                        var cateogryPackgesForSenders = category.Packages.GroupBy(p => p.Sender);
                        decimal numberOfSenders = cateogryPackgesForSenders.Count();
                        if (numberOfSenders > 1)
                            sender = "مختلف";
                        else
                            sender = category.Packages.FirstOrDefault().Sender.FullName;

                        weight += (int)category.Packages.Sum(p => p.Weight); 
                        transfers += (decimal)category.Packages.Sum(p => p.TransferPrice);
                        internalShipping += (decimal)category.Packages.Sum(p => p.InternalShipmentPrice);
                        casing += (decimal)category.Packages.Sum(p => p.CasedNumber);
                        packages += (int)category.Packages.Sum(p => p.PackagesCount);
                        totalPackages += packages;
                        totalWeight += weight; 
                    }

                    dt.Rows.Add(item.Key.FullName, packages.ToString(), customerByGroup.Key.Name, sender, weight.ToString(), item.Key.City.Name, transfers.ToString(), internalShipping.ToString(), casing.ToString()); 
                }
            }

            // Set the journe info 
            ReportDocument reprot = new ReportDocument();
            reprot.Load(Server.MapPath("~/Reports/CalculationsTableReport.rpt"));
            reprot.SetDataSource(ds);

            // SEt the journe yinfo 
            reprot.SetParameterValue("JourneyNumber", journey.JourneyNumber.ToString());
            reprot.SetParameterValue("JourneyDate", journey.JourneyDate.ToShortDateString());
            reprot.SetParameterValue("DriverName", journey.Driver.FullName);
            reprot.SetParameterValue("VehicleNumber", journey.Driver.VehicleNumber);
            reprot.SetParameterValue("TotalWeight", totalWeight.ToString());
            reprot.SetParameterValue("TotalPackages", totalPackages.ToString());

            // Return a file 
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = reprot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "جدول الحسابات " + journey.JourneyNumber + ".pdf"); 
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(404);
            }

        }

        // Get the detialed packages report 
        public ActionResult DetailedPackagesTable(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();

            // Create a dataset 
            DataSet ds = new DataSet();
            PakcagesTableDataTable dt = new PakcagesTableDataTable();
            ds.Tables.Add(dt);

            foreach (var item in journey.Packages)
            {
                dt.Rows.Add(item.Category.Name, item.Quantity.ToString(), item.Dozens.ToString(), item.Sender.FullName, item.PackageNumber, item.Weight.ToString(), item.Customer.FullName); 
            }

            // Set the data of the report 
            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Reports/DetailedPackagesTableReport.rpt"));

            report.SetDataSource(ds);
            report.SetParameterValue("JourneyNumber", journey.JourneyNumber.ToString());
            report.SetParameterValue("JourneyDate", journey.JourneyDate.ToShortDateString());
            report.SetParameterValue("SenderCompany", journey.SenderCompany.Name + " " + journey.SenderCompany.CommercialNumber.ToString());
            report.SetParameterValue("ReceiverCompany", journey.ReceiverCompany.Name + " " + journey.ReceiverCompany.CommercialNumber.ToString());

            // Return a file 
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "قائمة مفردات الطرود " + journey.JourneyNumber + ".pdf");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(404);
            }
        }

        //// Get customer envolope 
        //public ActionResult CustomerEnvelope(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return new HttpStatusCodeResult(406);

        //    var journey = db.Journeys.Find(id);
        //    if (journey == null)
        //        return HttpNotFound();

        //    var packagesByCity = journey.Packages.GroupBy(p => p.Customer.City);

        //    foreach (var item in packagesByCity)
        //    {
                
        //    }
        //}
    }
}