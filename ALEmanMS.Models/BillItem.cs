using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class BillItem
    {
        [Key]
        public string BillItemId { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        //Relation with other tables
        [Required]
        public virtual Bill Bill { get; set; }

        public string BillId { get; set; }

    }
}
