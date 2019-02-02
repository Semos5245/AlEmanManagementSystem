using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALEmanMS.Models
{
    public class UserActivity
    {

        [Key]
        public string UserActivityId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }


        public DateTime ActivityDate { get; set; }

    }
}
