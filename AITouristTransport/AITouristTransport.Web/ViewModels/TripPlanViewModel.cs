using System.ComponentModel.DataAnnotations;

namespace AITouristTransport.Web.ViewModels
{
    public class TripPlanViewModel
    {
        [Required(ErrorMessage = "Please select a starting location.")]
        public string StartLocation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a destination.")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a travel date.")]
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }

        [Required(ErrorMessage = "Please enter the number of travellers.")]
        [Range(1, 50, ErrorMessage = "Travellers must be between 1 and 50.")]
        public int Travellers { get; set; }

        [Required(ErrorMessage = "Please enter your budget.")]
        [Range(1, 10000000, ErrorMessage = "Please enter a valid budget.")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "Please select your luggage size.")]
        public string LuggageSize { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the terrain type.")]
        public string TerrainType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the travel duration.")]
        [Range(1, 30, ErrorMessage = "Travel duration must be between 1 and 30 days.")]
        public int TravelDurationDays { get; set; }
    }
}