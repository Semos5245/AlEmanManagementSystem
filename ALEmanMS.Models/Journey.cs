using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Journey
    {
        [Key]
        public string JourneyId { get; set; }

        public int JourneyNumber { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(100)]
        public string Policy { get; set; }

        public decimal? Customs { get; set; }

        public DateTime JourneyDate { get; set; }

        //Relation with other tables
        public  virtual List<Package> Packages { get; set; }

        public virtual List<Bill> Bills { get; set; }

        [Required]
        public virtual Driver Driver { get; set; }

        [ForeignKey("Driver")]
        public string DriverId { get; set; }

        [Required]
        public virtual SenderCompany SenderCompany { get; set; }

        public string SenderCompanyId { get; set; }

        [Required]
        public virtual ReceiverCompany ReceiverCompany { get; set; }

        public string ReceiverCompanyId { get; set; }

        [Required] 
        public virtual JourneyType JourneyType { get; set; }

        public string JourneyTypeId { get; set; }

        public decimal RyialPrice { get; set; }

        public decimal DollarPrice { get; set; }

    }
}
