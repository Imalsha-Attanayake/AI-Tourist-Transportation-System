using AITouristTransport.Web.ViewModels;

namespace AITouristTransport.Web.Services.Interfaces
{
    public interface IAIService
    {
        Task<AITripPlanResult> GenerateTripPlanAsync(TripPlanViewModel model);
    }
}