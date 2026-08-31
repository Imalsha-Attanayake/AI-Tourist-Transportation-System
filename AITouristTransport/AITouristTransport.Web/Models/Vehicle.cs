using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AITouristTransport.Web.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        public string VehicleName { get; set; } = string.Empty;

        [Required]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        public int SeatCapacity { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal PricePerDay { get; set; }

        public string? ImageUrl { get; set; }

        public int? ProviderId { get; set; }

        public VehicleProvider? Provider { get; set; }
    }
}