using ALEmanMS.AdminWebsite.Datasets;
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

                    //totalPackages += (int)item.Key.Packages.Sum(c => c.PackagesCount);
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
                dt.AddPakcagesTableRow(item.Category.Name, item.Quantity.ToString(), item.Dozens.ToString(), item.Sender.FullName, item.PackageNumber.Value.ToString(), Convert.ToInt32(item.Weight.Value), item.Customer.FullName, 0, 0, "0");
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
                return File(stream, "application/pdf", "قائمة مفردات الطرود التفصيلية " + journey.JourneyNumber + ".pdf");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        // Packages table without sender and customer 
        public ActionResult PackagesTable(string id)
        {
            // Validate 
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(406);

            // Get the journey 
            var journey = db.Journeys.Find(id);
            if (journey == null)
                return HttpNotFound();


            // Create a dataset 
            DataSet ds = new DataSet();
            PakcagesTableDataTable dt = new PakcagesTableDataTable();
            ds.Tables.Add(dt);

            foreach (var item in journey.Packages)
            {
                dt.AddPakcagesTableRow(item.Category.Name, item.Quantity.ToString(), item.Dozens.ToString(), item.Sender.FullName, item.PackageNumber.Value.ToString(), Convert.ToInt32(item.Weight.Value), item.Customer.FullName,0,0,"0");
            }

            // Set the data of the report 
            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Reports/PackagesTableReport.rpt"));

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
            catch (Exception)
            {
                return new HttpStatusCodeResult(500);
            }

        }

        // Get customer envolope 
        public ActionResult CustomerEnvelope(int? id)
        {
            // Valdiate the input 
            if (id == null)
                return new HttpStatusCodeResult(406);

            var bill = db.Bills.Find(id.Value);
            if (bill == null)
                return HttpNotFound();

            string phone1 = "";
            string phone2 = "";
            string phone3 = "";

            if (bill.Customer.Phone1 != null)
                phone1 = bill.Customer.Phone1;

            if (bill.Customer.Phone2 != null)
                phone2 = bill.Customer.Phone2;

            if (bill.Customer.Phone3 != null)
                phone3 = bill.Customer.Phone3;

            // Create the report 
            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Reports/CustomerEnvelope.rpt"));
            // Set the values 
            report.SetParameterValue("JourneyNumber", bill.Journey.JourneyNumber);
            report.SetParameterValue("PackagesCount", bill.PackageCount);
            report.SetParameterValue("Phone1", phone1);
            report.SetParameterValue("Phone2", phone2);
            report.SetParameterValue("Telephone", phone3);
            report.SetParameterValue("CityName", bill.City);
            report.SetParameterValue("CustomerName", bill.Customer.FullName + " - " + bill.Customer.City.Name);

            Response.Buffer = false;
            Response.ClearHeaders();
            Response.ClearContent();

            try
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ظرف بوليصة " + bill.BillId + ".pdf"); 
            }
            catch (Exception)
            {
                return HttpNotFound(); 
            }

        }

        // Generate the customer bills 
        public ActionResult CustomerBill(int? id)
        {
            // validate the 
            if (id == null)
                return new HttpStatusCodeResult(406);

            // get the bill 
            var bill = db.Bills.Find(id.Value);
            if (bill == null)
                return HttpNotFound();

            // Get the journey 
            var journey = bill.Journey;

            // Get the bill Senders 
            var billSenders = bill.BillSenders;

            // Create the dataset 
            DataSet ds = new DataSet();

            // Create the datatable 
            BillSendersDataTable dt = new BillSendersDataTable();
            // Add the items to the table 
            foreach (var item in billSenders)
            {
                dt.AddBillSendersRow(item.Description, item.TotalPackages.ToString());
            }

            // Get the ibll items 
            var billItems = bill.BillItems;

            // Create the datatable for bill descriptions 
            BillDescriptionsDataTable bdDt = new BillDescriptionsDataTable();

            // Addthe bill descriptions to datatabe 
            foreach (var item in billItems)
            {
                bdDt.AddBillDescriptionsRow(item.Amount.ToString("#"), item.Description, "");
            }

            // Create a sender packages table 
            PakcagesTableDataTable pdt = new PakcagesTableDataTable();

            // Add the data to the packages table 
            foreach (var item in journey.Packages.Where(p => p.Customer == bill.Customer))
            {
                pdt.AddPakcagesTableRow(item.Category.Name, item.Quantity.ToString(), item.Dozens.ToString()
                    , item.Sender.FullName, item.PackageNumber.ToString(), Convert.ToInt32(item.Weight.Value), item.Customer.FullName, Convert.ToInt32(item.TransferPrice), item.CasedNumber.Value, "0");
            }
            // ADdd the table to the dataset 
            ds.Tables.Add(pdt); 

            // Add the table to the dataste 
            ds.Tables.Add(dt);
            ds.Tables.Add(bdDt); 

            // Create the report 
            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Reports/BillReport.rpt"));
            //report.SetDataSource(ds);
            // Set the datasource of the subreport 
            report.Subreports[0].SetDataSource(ds);
            report.Subreports[1].SetDataSource(ds);
            report.Subreports[2].SetDataSource(ds);
            // Set parameters 
            report.SetParameterValue("BillId", id.Value);
            report.SetParameterValue("CustomerName", bill.Customer.FullName);
            report.SetParameterValue("CityName", bill.Customer.City.Name);
            report.SetParameterValue("JourneyNumber", bill.JourneyNumber.ToString());
            report.SetParameterValue("JourneyDate", bill.JourneyDate.ToString("dd/mm/yyyy"));
            report.SetParameterValue("GroupName", bill.Group);
            report.SetParameterValue("Weight", bill.Weight.ToString());
            report.SetParameterValue("PackagesCount", bill.PackageCount.ToString());
            report.SetParameterValue("TotalValue", bill.Total.ToString("#"), report.Subreports[0].Name);
            report.SetParameterValue("TotalValueInSR", bill.TotalInSR.ToString("#"), report.Subreports[0].Name);
            report.SetParameterValue("TotalValueWords", "ثلاثئمة وواحد ريال سعودي", report.Subreports[0].Name);

            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "appliction/pdf", "بوليصة شحن " + id.Value + ".pdf"); 
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500); 
            }


        }

        // Generate manifest depends on the city 
        public ActionResult Manifest(string destinationId, string journeyId)
        {
            // validate the input 
            if (string.IsNullOrEmpty(destinationId) || string.IsNullOrEmpty(journeyId))
                return new HttpStatusCodeResult(406);

            // Get the journey 
            var journey = db.Journeys.Find(journeyId);
            if (journey == null)
                return HttpNotFound();

            // get the destination 
            var destination = db.Destinations.Find(destinationId);
            if (destination == null)
                return HttpNotFound();

            // Get the bills 
            var bill = journey.Bills.Where(b => b.Customer.City.Destination == destination).ToList(); 

            // Create the dataset 
            DataSet ds = new DataSet();

            JourneyManifestDataTable dt = new JourneyManifestDataTable();
            ds.Tables.Add(dt); 
            // Add the data to the row 
            foreach (var item in bill)
            {
                decimal shippingPrice = (decimal)item.BillItems.Where(b => b.Description == "أجور الشحن").Sum(b => b.Amount);
                decimal transfer = (decimal)item.BillItems.Where(b => b.Description.Contains("قيمة بضاعة")).Sum(b => b.Amount);
                decimal casingPrice = (decimal)item.BillItems.Where(b => b.Description.Contains("خيش")).Sum(b => b.Amount);
                decimal additionalAmounts = (decimal)item.BillItems.Where(b => b.Description != "أجور الشحن" && !b.Description.Contains("خيش") && !b.Description.Contains("قيمة بضاعة")).Sum(b => b.Amount);
                
                dt.AddJourneyManifestRow(item.PackageCount.ToString(), item.Customer.FullName, item.City, item.Total.ToString("#"),
                                         item.Weight.ToString(), item.BillId.ToString(), item.Customer.Phone3,  item.Notes, shippingPrice.ToString("#"), casingPrice.ToString("#"), transfer.ToString("#"), additionalAmounts.ToString("#")); 
            }

            // Calculate total packages and amount 
            string totalPackages = bill.Sum(b => b.PackageCount).ToString("#");
            string totalAmount = bill.Sum(b => b.Total).ToString("#");

            // Create the report 
            ReportDocument report = new ReportDocument();

            report.Load(Server.MapPath("~/Reports/DestinationManifestReport.rpt"));
            // set he datasource 
            report.SetDataSource(ds);

            // SEt the parameters 
            report.SetParameterValue("ManifestName", "منفست " + destination.Name);
            report.SetParameterValue("JourneyNumber", journey.JourneyNumber.ToString());
            report.SetParameterValue("JourneyDate", journey.JourneyDate.ToString("dd/mm/yyyy"));
            report.SetParameterValue("DriverName", journey.Driver.FullName);
            report.SetParameterValue("VehicleNumber", journey.Driver.VehicleNumber.ToString());
            report.SetParameterValue("TotalPackages", totalPackages);
            report.SetParameterValue("TotalAmount", totalAmount);

            // Genereate hte repsonse 
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "منفست " + destination.Name + " للرحلة " + journey.JourneyNumber + ".pdf"); 
            }
            catch (Exception ex) 
            {
                return new HttpStatusCodeResult(500); 
            }
        }
    }
}