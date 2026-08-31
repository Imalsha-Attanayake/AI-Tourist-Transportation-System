using AITouristTransport.Web.Models;
using AITouristTransport.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AITouristTransport.Web.Controllers
{
    [Authorize(Roles = "VehicleProvider")]
    public class VehicleProviderController : Controller
    {
        private readonly IVehicleProviderService _vehicleProviderService;
        private readonly IVehicleService _vehicleService;
        private readonly IBookingService _bookingService;

        public VehicleProviderController(
            IVehicleProviderService vehicleProviderService,
            IVehicleService vehicleService,
            IBookingService bookingService)
        {
            _vehicleProviderService = vehicleProviderService;
            _vehicleService = vehicleService;
            _bookingService = bookingService;
        }

        // Display provider dashboard
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider != null)
            {
                var vehicles =
                    await _vehicleService.GetVehiclesByProviderIdAsync(
                        provider.ProviderId
                    );

                ViewBag.MyVehicles = vehicles;
            }

            return View(provider);
        }


        // Display provider profile creation page
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // Process provider profile creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleProvider provider)
        {
            if (!ModelState.IsValid)
            {
                return View(provider);
            }

            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var existingProvider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (existingProvider != null)
            {
                ModelState.AddModelError(
                    "",
                    "A vehicle provider profile already exists for your account."
                );

                return View(provider);
            }

            provider.UserId = userId;

            await _vehicleProviderService.AddAsync(provider);

            return RedirectToAction(nameof(Index));
        }


        // Display bookings received by the provider
        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return NotFound();
            }

            var bookings =
                await _bookingService.GetBookingsByProviderIdAsync(
                    provider.ProviderId
                );

            return View(bookings);
        }


        // Approve a booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return NotFound();
            }

            var booking =
                await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            // Make sure this booking belongs to the logged-in provider
            if (booking.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            booking.Status = "Approved";

            await _bookingService.UpdateBookingAsync(booking);

            return RedirectToAction(nameof(Bookings));
        }


        // Reject a booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var userId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var provider =
                await _vehicleProviderService.GetByUserIdAsync(userId);

            if (provider == null)
            {
                return NotFound();
            }

            var booking =
                await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            // Make sure this booking belongs to the logged-in provider
            if (booking.ProviderId != provider.ProviderId)
            {
                return Forbid();
            }

            booking.Status = "Rejected";

            await _bookingService.UpdateBookingAsync(booking);

            return RedirectToAction(nameof(Bookings));
        }
    }
}