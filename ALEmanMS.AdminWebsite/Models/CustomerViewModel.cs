using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Models
{
    public class CustomerViewModel
    {
        [Required]
        [StringLength(25)]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "الكنية")]
        public string LastName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        [Required]
        [Display(Name = "المدينة")]
        public string CityId { get; set; }

        [Display(Name = "التولد")]
        public DateTime BirthDate { get; set; }

        [StringLength(512)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "دفع في دمشق")]
        public string PaidInDamascus { get; set; }

        [Display(Name = "سعر الكيلو")]
        public decimal SpecialFee { get; set; } = 0;

        [StringLength(50)]
        [Display(Name = "الشركة")]
        public string Company { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "الاسم التجاري")]
        public string CommercialName { get; set; }

        [StringLength(15)]
        [Display(Name = "الرقم الضريبي")]
        public string TaxtNumber { get; set; }

        public SelectList Cities { get; set; }

    }
}