using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class SaudiJourneyCustom
    {
        [Key]
        public string JourneyCustomID { get; set; }

        public int JourneyNumber { get; set; }

        public DateTime JourneyDate { get; set; }

        public string SenderCompany { get; set; }

        public string ReceiverCompany { get; set; }

        public int Total { get; set; }

        public int TotalDozen { get; set; }

        public int TotalPrice { get; set; }

        public int TotalWeight { get; set; }

        public int TotalNetWeight { get; set; }

        public double WeightPortion { get; set; }

        // Relation with the journey table 
        public string JourneyId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }

        public virtual List<SaudiCustomItem> SaudiCustomItems { get; set; }

    }
}
