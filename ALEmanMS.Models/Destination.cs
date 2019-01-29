using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Destination
    {
        [Key]
        public string DestinationId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Relationsships with other tables 
        public virtual List<City> Cities { get; set; }
    }
}
