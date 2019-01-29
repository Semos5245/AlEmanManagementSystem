using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALEmanMS.Models
{
    [Table("Senders")]
    public class Sender : Person
    {
        [StringLength(20)]
        public string NationalID { get; set; }

        [StringLength(20)]
        public string MotherName { get; set; }

        [StringLength(50)]
        public string Profession { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        // Relationships with other tables 
        public virtual List<Journey> Journeys { get; set; }

    }
}
