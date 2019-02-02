using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALEmanMS.Models
{
    [Table("Drivers")]
    public class Driver : Person
    {
        [StringLength(20)]
        public string VehicleNumber { get; set; }

        public double? VehicleWeight { get; set; }

        public double? VehicleFrontHeight { get; set; }

        public double? VehicleRearHight { get; set; }

        public double? VehicleSize { get; set; }

        public double? VehicleLength { get; set; }

        // Relationships with other tables 
        public virtual List<Journey> Journeys { get; set; }

    }
}
