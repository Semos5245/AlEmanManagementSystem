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

        //Relation with other tables
        public virtual List<GroupingGroup> GroupingGroups { get; set; }

        public virtual List<Category> Categories { get; set; }

    }
}
