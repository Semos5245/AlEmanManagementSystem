using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Contact
    {
        [Key]
        public string ContactId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Reltionship with other tables 
        public List<PersonContact> PersonContacts { get; set; }
    }
}
