using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //Relation with other tables
        [Required]
        public virtual Group Group { get; set; }

        public string GroupId { get; set; }

        [Required]
        public virtual GroupingGroup GroupingGroup { get; set; }

        public string GroupingGroupId { get; set; }

        public virtual Unit Unit { get; set; }

        public string UnitId { get; set; }

        public virtual List<Package> Packages { get; set; }

    }
}
