using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.AdminWebsite.Models
{
    public class SenderCompanyViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "اسم الشركة")]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "المدينة")]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "الرقم التجاري")]
        public string CommercialNumber { get; set; }
        
        [StringLength(50)]
        [Display(Name = "هاتف")]
        public string Phone { get; set; }

        [StringLength(512)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }

    }
}