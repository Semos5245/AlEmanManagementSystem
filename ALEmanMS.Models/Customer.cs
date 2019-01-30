using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    [Table("Customers")]
    public class Customer : Person
    {
        public decimal SpecialFee { get; set; } = 0; 

        public bool PaidInDamas { get; set; } = false;
        
        [StringLength(50)]
        public string Company { get; set; }

        [Required]
        [StringLength(50)]
        public string CommercialName { get; set; }

        [StringLength(15)]
        public string TaxNumber { get; set; }

        // Realtions ships with other tables 
        public virtual List<Journey> Journeys { get; set; }

        [Required]
        public virtual City City { get; set; }

        public string CityId { get; set; }
        
    }
}
