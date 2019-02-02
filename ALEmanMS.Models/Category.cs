using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual List<Package> Packages { get; set; }

        [Required]
        public virtual Group Group { get; set; }

        public string GroupId { get; set; }
        
        public virtual GroupingGroup GroupingGroup { get; set; }

        [ForeignKey("GroupingGroup")]
        public string GroupingGroupId { get; set; }

        //[Required]
        public virtual GroupingGroup LowGroupingGroup { get; set; }

        [ForeignKey("LowGroupingGroup")]
        public string LowGroupingGroupId { get; set; }

        [Required]
        public virtual Unit Unit { get; set; }

        public string UnitId { get; set; }


    }
}
