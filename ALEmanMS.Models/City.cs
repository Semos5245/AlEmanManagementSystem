using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class City
    {
        [Key]
        public string CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public int CityOrder { get; set; } = 0; 

        // Relationships with other tables 
        [Required]
        public virtual Destination Destination { get; set; }

        public string DestinationId { get; set;  }
    }
}
