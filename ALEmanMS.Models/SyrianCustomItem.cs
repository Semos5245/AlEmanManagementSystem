using System.ComponentModel.DataAnnotations;

namespace ALEmanMS.Models
{
    public class SyrianCustomItem
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
        public virtual SyrianJourneyCustom SyrianJourneyCustom { get; set; }

        public string SyrianJourneyCustomId { get; set; }

    }
}
