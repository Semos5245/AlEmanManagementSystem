using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class ReceiverCompany
    {
        [Key]
        public string ReceiverCompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Country { get; set; }
        
        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(25)]
        public string CommercialNumber { get; set; }

        //Relation with other tables
        [Required]
        public virtual List<Journey> Journeys { get; set; }
        
    }
}
