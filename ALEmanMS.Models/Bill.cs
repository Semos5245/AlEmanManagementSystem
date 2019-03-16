using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Bill
    {
        
        public int BillId { get; set; }

        public DateTime JourneyDate { get; set; }
        
        [StringLength(50)]
        public string CarNumber { get; set; }

        public int PackageCount { get; set; }
        
        [StringLength(50)]
        public string DriverName { get; set; }

        public decimal Total { get; set; }

        public decimal TotalInSR { get; set; }

        public bool PaidInDamas { get; set; } = false;

        public double ShipmentPrice { get; set; }

        public double Weight { get; set; }

        public double PlaneWeight { get; set; }

        public int JourneyNumber { get; set; }

        public int PackagingCount { get; set; }

        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Group { get; set; }

        [StringLength(512)]
        public string Notes { get; set; }
        

        //Relation with other tables
        public virtual List<BillItem> BillItems { get; set; }

        public virtual List<BillSender> BillSenders { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }

        
        public string JourneyId { get; set; }
        
    }
}
