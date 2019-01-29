using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Unit
    {
        [Key]
        public string UnitId { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

    }
}
