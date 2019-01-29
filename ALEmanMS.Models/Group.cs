using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Group
    {
        [Key]
        public string GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Relation with othrer tables
        public virtual List<Category> Categories { get; set; }

    }
}
