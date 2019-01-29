using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class PersonContact
    {
        [Key]
        public string PersonContactId { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }

        // Realtionship with other tables 
        [Required]
        public virtual Contact Contact { get; set; }

        public string ContactId { get; set; }

        [Required]
        public virtual Person Person { get; set; }

        public string PersonId { get; set; }


    }
}
