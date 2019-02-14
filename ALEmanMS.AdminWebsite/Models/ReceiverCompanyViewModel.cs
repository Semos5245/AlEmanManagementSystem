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
        [Display(Name = "اسم الشركة")]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "المدينة")]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "الرقم الوطني")]
        public string CommercialNumber { get; set; }

        [StringLength(512)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }
    }
}