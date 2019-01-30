using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class SenderCompany
    {
        [Key]
        public string SenderCompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(25)]
        public string CommercialNumber { get; set; }

        //Relation with other tables
        public virtual List<Journey> Journeys { get; set; }

    }
}
