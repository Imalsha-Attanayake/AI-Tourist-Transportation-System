using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Services.Interfaces;
using System.Security.Claims;

namespace AITouristTransport.Web.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IVehicleService _vehicleService;
        private readonly IReviewService _reviewService;

        public BookingController(
            IBookingService bookingService,
            IVehicleService vehicleService,
            IReviewService reviewService)
        {
            _bookingService = bookingService;
            _vehicleService = vehicleService;
            _reviewService = reviewService;
        }

        // Display booking form
        [HttpGet]
        public async Task<IActionResult> Create(int vehicleId)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.Vehicle = vehicle;

            return View();
        }


        // Process booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int VehicleId,
            string StartLocation,
            string Destination,
            DateTime TravelDate,
            int TravelDurationDays,
            int Travellers)
        {
            // Get the logged-in user's ID
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "User");
            }

            // Get selected vehicle
            var vehicle = await _vehicleService.GetVehicleByIdAsync(VehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }

            // Calculate total booking amount
            decimal totalAmount =
                vehicle.PricePerDay * TravelDurationDays;

            // Create booking
            var booking = new Booking
            {
                UserId = userId,
                VehicleId = vehicle.VehicleId,
                ProviderId = vehicle.ProviderId ?? 0,

                StartLocation = StartLocation,
                Destination = Destination,
                TravelDate = TravelDate,
                TravelDurationDays = TravelDurationDays,
                Travellers = Travellers,

                TotalAmount = totalAmount,

                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            await _bookingService.AddBookingAsync(booking);

            return RedirectToAction(nameof(History));
        }


        // Display user's booking history
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "User");
            }

            var bookings =
                await _bookingService.GetBookingsByUserIdAsync(userId);

            // Get reviews submitted by the logged-in tourist
            var reviews =
                await _reviewService.GetReviewsByUserIdAsync(userId);

            // Store booking IDs that already have reviews
            var reviewedBookingIds =
                reviews.Select(r => r.BookingId).ToHashSet();

            ViewBag.ReviewedBookingIds = reviewedBookingIds;

            return View(bookings);
        }
    }
}