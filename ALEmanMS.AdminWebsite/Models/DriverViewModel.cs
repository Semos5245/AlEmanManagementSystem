using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class DriverViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "الاسم الاول")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "الكنية")]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "المدينة")]
        public string CityName { get; set; }

        [StringLength(512)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "التولد")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "رقم الآلية")]
        public string VehicleNumber { get; set; }

        [Display(Name = "طول الآلية")]
        public double? VehicleLength { get; set; }
        
        [Display(Name = "ارتفاع الآلية من الأمام")]
        public double? VehicleFrontHeight { get; set; }

        [Display(Name = "ارتفاع الآلية من الخلف")]
        public double? VehicleRearHeight { get; set; }

        [Display(Name = "حجم الآلية")]
        public double? VehicleSize { get; set; }

    }
}