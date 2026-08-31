using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AITouristTransport.Web.Services.Interfaces;
using AITouristTransport.Web.ViewModels;

namespace AITouristTransport.Web.Controllers
{
    public class TripController : Controller
    {
        private readonly IAIService _aiService;

        public TripController(IAIService aiService)
        {
            _aiService = aiService;
        }

        // Process trip planning and generate AI recommendations
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Plan(TripPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Send the trip information to the AI service
                var aiResult =
                    await _aiService.GenerateTripPlanAsync(model);

                // Store the complete AI result temporarily
                TempData["Destination"] = model.Destination;
                TempData["StartLocation"] = model.StartLocation;
                TempData["TravelDate"] =
                    model.TravelDate.ToString("dd MMMM yyyy");
                TempData["Travellers"] =
                    model.Travellers.ToString();
                TempData["Budget"] =
                    model.Budget.ToString("N2");
                TempData["TravelDurationDays"] =
                    model.TravelDurationDays.ToString();
                TempData["LuggageSize"] =
                    model.LuggageSize;
                TempData["TerrainType"] =
                    model.TerrainType;

                TempData["RecommendedVehicle"] =
                    aiResult.RecommendedVehicle;

                TempData["EstimatedCostLkr"] =
                    aiResult.EstimatedCostLkr.ToString("N2");

                TempData["Route"] =
                    string.Join(" → ", aiResult.Route);

                TempData["TotalDistanceKm"] =
                    aiResult.TotalDistanceKm.ToString("N2");

                return RedirectToAction(nameof(Result));
            }
            catch (Exception)
            {
                TempData["AIError"] =
                    "Unable to generate AI travel recommendations. " +
                    "Please make sure the AI service is running.";

                return RedirectToAction(nameof(Result));
            }
        }

        // Display trip planning result
        [Authorize]
        [HttpGet]
        public IActionResult Result()
        {
            return View();
        }
    }
}