using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Models
{
    public class SenderViewModel
    {
        [Required]
        [StringLength(25)]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        [StringLength(25)]
        [Display(Name = "اسم الأم")]
        public string MotherName { get; set; }

        [StringLength(50)]
        [Display(Name = "المهنة")]
        public string Profession { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "الشركة")]
        public string Company { get; set; }

        [StringLength(20)]
        [Display(Name = "البلد")]
        public string State { get; set; }

        [StringLength(20)]
        [Display(Name = "الرقم الوطني")]
        public string NationalId { get; set; }

        [StringLength(50)]
        [Display(Name = "رقم التسجيل")]
        public string RegisterationNumber { get; set; }

        [Display(Name = "التولد")]
        public DateTime BirthDate { get; set; }

        [StringLength(512)]
        [Display(Name = "ملاحظات")]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "المدينة")]
        public string CityId { get; set; }

        public SelectList Cities { get; set; }
    }
}