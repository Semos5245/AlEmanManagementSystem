using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class PackageViewModel
    {
        public string CategoryId { get; set; }

        public int Count { get; set; }

        public int Dozen { get; set; }

        public int Kilo { get; set; }

        public int PackageCounts { get; set; }

        public int PackageNumber { get; set; }

        public double Weight { get; set; }

        public string SenderId { get; set; }

        public double PlaneWeight { get; set; }

        public int CasedOriginalNumber { get; set; }

        public int CasedNumber { get; set; }

        public decimal Transfer { get; set; }

        public decimal InternalShipping { get; set; }

        public string CustomerId { get; set; }
    }
}