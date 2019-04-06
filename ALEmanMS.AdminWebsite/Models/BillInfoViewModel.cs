using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class BillInfoViewModel
    {

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        public int JourneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string DriverName { get; set; }

        [Required]
        [StringLength(50)]
        public string CarNumber { get; set; }

        //[Required]
        [StringLength(250)]
        public string Notes { get; set; }

        [Required]
        [StringLength(50)]
        public string Group { get; set; }


        [Required]
        public DateTime JourneyDate { get; set; }

        [Required]
        public int PackageCount { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public decimal TotalInSR { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public int PlaneWeight { get; set; }

        [Required]
        public decimal ShipmentPrice { get; set; }

        [Required]
        public int PackagingCount { get; set; }

        [Required]
        public bool PaidInDamas { get; set;  }

    }
}