namespace AITouristTransport.Web.ViewModels
{
    public class AITripPlanResult
    {
        public string RecommendedVehicle { get; set; } = string.Empty;

        public decimal EstimatedCostLkr { get; set; }

        public List<string> Route { get; set; } = new List<string>();

        public double TotalDistanceKm { get; set; }
    }
}