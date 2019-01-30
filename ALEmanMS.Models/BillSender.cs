using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class BillSender
    {
        [Key]
        public string BillSenderId { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        public int TotalPackages { get; set; }

        //Relation with other tables
        [Required]
        public virtual Bill Bill { get; set; }

        public string BillId { get; set; }

    }
}
