using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class PersonContactViewModel
    {
        [Required]
        [StringLength(40)]
        [Display(Name = "الاسم")]
        public string ContactName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "القيمة")]
        public string Value { get; set; }

        public Person Person { get; set; }

        public List<PersonContact> PersonContacts { get; set; }

    }
}