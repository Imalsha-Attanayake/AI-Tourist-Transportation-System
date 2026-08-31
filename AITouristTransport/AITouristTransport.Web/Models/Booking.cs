using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AITouristTransport.Web.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        // Tourist who makes the booking
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Vehicle selected for the booking
        [Required]
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }

        // Provider of the selected vehicle
        [Required]
        public int ProviderId { get; set; }

        [ForeignKey("ProviderId")]
        public VehicleProvider? Provider { get; set; }

        // Trip information
        [Required]
        public string StartLocation { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }

        [Required]
        [Range(1, 30)]
        public int TravelDurationDays { get; set; }

        [Required]
        [Range(1, 50)]
        public int Travellers { get; set; }

        // Booking price
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Booking status
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Booking creation date
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}