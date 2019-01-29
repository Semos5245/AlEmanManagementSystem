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

        public bool IsSeen { get; set; }

        [Required]
        public string FromId { get; set; }

        [Required]
        public string ToId { get; set; }

        public DateTime Date { get; set; }

        //Relation with other tables
        [Required]
        public virtual Person Person { get; set; }

        public string PersonId { get; set; }

    }
}
