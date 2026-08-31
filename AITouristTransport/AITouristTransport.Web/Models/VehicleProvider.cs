using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AITouristTransport.Web.Models
{
    public class VehicleProvider
    {
        [Key]
        public int ProviderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
    }
}