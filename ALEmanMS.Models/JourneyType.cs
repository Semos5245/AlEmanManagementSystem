using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class JourneyType
    {

        [Key]
        public string JourneyTypeId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        // Relaiton ship with other tables 
        public virtual List<Journey> Journeys { get; set;  }

    }
}
