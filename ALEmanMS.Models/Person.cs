using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public abstract class Person
    {
        [Key]
        public string PersonId { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(25)]
        public string CityName { get; set; }
        
        [StringLength(512)]
        public string Description { get; set; }

        public DateTime Birthdate { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }

        // Relationships with other tables 
        public virtual List<PersonContact> PersonContacts { get; set;  }

    }
}
