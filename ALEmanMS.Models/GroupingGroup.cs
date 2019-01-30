using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class GroupingGroup
    {
        [Key]
        public string GroupingGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        //Relation with other tables
        public virtual List<Category> Categories { get; set; }

        [Required]
        public virtual Unit Unit { get; set; }

        public string Unitid { get; set; }
    }
}
