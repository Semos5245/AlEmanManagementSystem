using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Models
{
    public class JourneyViewModel
    {
        [Required]
        [Display(Name = "رقم الرحلة")]
        public int JourneyNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "الموقع")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "تاريخ الرحلة")]
        public DateTime JourneyDate { get; set; }

        [StringLength(50)]
        [Display(Name = "السياسة")]
        public string Policy { get; set; }
        
        [Display(Name = "رسوم الحديثة")]
        public decimal Customs { get; set; }

        [Display(Name = "سعر الدولار")]
        public decimal DollarPrice { get; set; }

        [Display(Name = "سعر الريال")]
        public decimal RyialPrice { get; set; }

        [Required]
        [Display(Name = "السائق")]
        public string DriverId { get; set; }

        [Required]
        [Display(Name = "الشركة المرسلة")]
        public string SenderCompanyId { get; set; }

        [Required]
        [Display(Name = "الشركة المستقبلة")]
        public string ReceiverCompanyId { get; set; }

        [Required]
        [Display(Name = "نوع الرحلة")]
        public string JourneyTypeId { get; set; }


        public SelectList Drivers { get; set; }

        public SelectList JourneyTypes { get; set; }

        public SelectList SenderCompanies { get; set; }

        public SelectList ReceiverCompanies { get; set; }

        public string Message { get; set; } = "";

        public List<Package> Packages { get; set; }

        public Journey Journey { get; set; }

        public List<Sender> Senders { get; set; }

        public List<Customer> Customers { get; set; }

        public List<Category> Categories { get; set; }

        public List<Driver> DriversList { get; set; }

        public List<SenderCompany> SenderCompaniesList { get; set; }

        public List<ReceiverCompany> ReceiverCompaniesList { get; set; }

        public List<JourneyType> journeyTypesList { get; set; }
    }
}