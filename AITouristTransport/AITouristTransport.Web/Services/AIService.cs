using System.Net.Http.Json;
using System.Text.Json.Serialization;
using AITouristTransport.Web.Services.Interfaces;
using AITouristTransport.Web.ViewModels;

namespace AITouristTransport.Web.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AITripPlanResult> GenerateTripPlanAsync(
            TripPlanViewModel model)
        {
            // -------------------------------------------------
            // 1. Vehicle Recommendation - Random Forest
            // -------------------------------------------------

            var vehicleRequest = new
            {
                passengers = model.Travellers,
                budget_lkr = model.Budget,
                travel_duration_days = model.TravelDurationDays,
                luggage_size = model.LuggageSize,
                terrain_type = model.TerrainType
            };

            var vehicleResponse =
                await _httpClient.PostAsJsonAsync(
                    "api/vehicle-recommendation",
                    vehicleRequest);

            vehicleResponse.EnsureSuccessStatusCode();

            var vehicleResult =
                await vehicleResponse.Content
                    .ReadFromJsonAsync<VehicleRecommendationResponse>();


            // -------------------------------------------------
            // 2. Route Optimization - Dijkstra
            // -------------------------------------------------

            var routeRequest = new
            {
                start_location = model.StartLocation,
                destination = model.Destination
            };

            var routeResponse =
                await _httpClient.PostAsJsonAsync(
                    "api/route-optimization",
                    routeRequest);

            routeResponse.EnsureSuccessStatusCode();

            var routeResult =
                await routeResponse.Content
                    .ReadFromJsonAsync<RouteOptimizationResponse>();


            // -------------------------------------------------
            // 3. Budget Estimation - Linear Regression
            // -------------------------------------------------

            // The current budget dataset uses fuel prices
            // between 350 and 450 LKR.
            // 400 LKR is used as the system fuel-price value.

            var budgetRequest = new
            {
                vehicle_type = vehicleResult?.RecommendedVehicle ?? "",
                distance_km = routeResult?.TotalDistanceKm ?? 0,
                travel_days = model.TravelDurationDays,
                fuel_price = 400,
                passengers = model.Travellers
            };

            var budgetResponse =
                await _httpClient.PostAsJsonAsync(
                    "api/budget-estimation",
                    budgetRequest);

            budgetResponse.EnsureSuccessStatusCode();

            var budgetResult =
                await budgetResponse.Content
                    .ReadFromJsonAsync<BudgetEstimationResponse>();


            // -------------------------------------------------
            // Return combined AI result
            // -------------------------------------------------

            return new AITripPlanResult
            {
                RecommendedVehicle =
                    vehicleResult?.RecommendedVehicle ?? "Not available",

                EstimatedCostLkr =
                    budgetResult?.EstimatedCostLkr ?? 0,

                Route =
                    routeResult?.Route ?? new List<string>(),

                TotalDistanceKm =
                    routeResult?.TotalDistanceKm ?? 0
            };
        }


        // -----------------------------------------------------
        // Response classes
        // -----------------------------------------------------

        private class VehicleRecommendationResponse
        {
            [JsonPropertyName("recommended_vehicle")]
            public string RecommendedVehicle { get; set; }
                = string.Empty;
        }


        private class BudgetEstimationResponse
        {
            [JsonPropertyName("estimated_cost_lkr")]
            public decimal EstimatedCostLkr { get; set; }
        }


        private class RouteOptimizationResponse
        {
            [JsonPropertyName("route")]
            public List<string> Route { get; set; }
                = new List<string>();

            [JsonPropertyName("total_distance_km")]
            public double TotalDistanceKm { get; set; }
        }
    }
}