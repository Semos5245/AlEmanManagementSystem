using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class Package
    {
        [Key]
        public string PackageId { get; set; }

        public int? Quantity { get; set; }

        public int? Dozens { get; set; } = 0;

        public double? Weight { get; set; } = 0;

        public int? Kilograms { get; set; } = 0;

        public int? PackageNumber { get; set; }

        public decimal? TransferPrice { get; set; }

        public decimal? InternalShipmentPrice { get; set; } = 0;

        public decimal? PackagingPrice { get; set; } = 0;

        public double? PlaneWeight { get; set; } = 0;

        public int? CasedOriginalNumber { get; set; } = 0;

        public int? CasedNumber { get; set; }

        //Relation with other tables
        [Required]
        public virtual Sender Sender { get; set; }

        public string SenderId { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        public string CustomerId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }

        public string JourneyId { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public string CategoryId { get; set; }
        
    }
}
