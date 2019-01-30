using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class Message
    {
        [Key]
        public string MessageId { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        public bool IsSeen { get; set; } = false;

        [Required]
        [StringLength(50)]
        public string FromId { get; set; }

        [Required]
        [StringLength(50)]
        public string ToId { get; set; }

        public DateTime MessageDate { get; set; }

        //Relation with other tables
    }
}
