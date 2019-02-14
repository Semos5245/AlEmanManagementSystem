using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class ReceiverCompanyViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        public string CommercialNumber { get; set; }

        [StringLength(512)]
        public string Description { get; set; }
    }
}