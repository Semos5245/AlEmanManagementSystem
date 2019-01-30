using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Journey
    {
        [Key]
        public string JourneyId { get; set; }
        
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

        public string DriverId { get; set; }

        [Required]
        public virtual SenderCompany SenderCompany { get; set; }

        public string SenderCompanyId { get; set; }

        [Required]
        public virtual ReceiverCompany ReceiverCompany { get; set; }

        public string ReceiverId { get; set; }

    }
}
