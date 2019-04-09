using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    // Saudi Item 
    public class SaudiCustomItem
    {
        [Key]
        public string ItemId { get; set; }

        public int Total { get; set; }

        public string Cateogry { get; set; }

        public decimal CategoryPrice { get; set; }

        public int Count { get; set; }

        public int Dozen { get; set; }

        public int Price { get; set; }

        public int Weight { get; set; }

        public int NetWeight { get; set; }

        public int PackagesCount { get; set; }

        public string SenderName { get; set; }

        // Relaitons 
        [Required]
        public virtual SaudiJourneyCustom SaudiJourneyCustom { get; set; }

        public string JourneyCustomId { get; set; }
    }
}
