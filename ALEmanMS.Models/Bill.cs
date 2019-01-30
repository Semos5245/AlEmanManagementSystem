using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Bill
    {
        [Key]
        public string BillId { get; set; }

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

        [StringLength(512)]
        public string Notes { get; set; }

        //Relation with other tables
        public virtual List<BillItem> BillItems { get; set; }

        public virtual List<BillSender> BillSenders { get; set; }

        [Required]
        public virtual Customer Customer { get; set; }

        public string CustomerId { get; set; }

        [Required]
        public virtual Group Group { get; set; }

        public string GroupId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }

        public string JourneyId { get; set; }
        
    }
}
