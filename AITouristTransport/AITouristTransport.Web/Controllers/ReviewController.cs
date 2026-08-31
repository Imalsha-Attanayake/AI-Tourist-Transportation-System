using AITouristTransport.Web.Models;
using AITouristTransport.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AITouristTransport.Web.Controllers
{
    [Authorize(Roles = "Tourist")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBookingService _bookingService;

        public ReviewController(
            IReviewService reviewService,
            IBookingService bookingService)
        {
            _reviewService = reviewService;
            _bookingService = bookingService;
        }

        // Display review form
        [HttpGet]
        public async Task<IActionResult> Create(int bookingId)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "User");
            }

            var booking =
                await _bookingService.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // Make sure the booking belongs to the logged-in tourist
            if (booking.UserId != userId)
            {
                return Forbid();
            }

            // Reviews can only be submitted for approved bookings
            if (booking.Status != "Approved")
            {
                TempData["ReviewError"] =
                    "You can only review an approved booking.";

                return RedirectToAction("History", "Booking");
            }

            // Prevent duplicate reviews
            if (await _reviewService.HasReviewAsync(bookingId))
            {
                TempData["ReviewError"] =
                    "You have already submitted a review for this booking.";

                return RedirectToAction("History", "Booking");
            }

            ViewBag.Booking = booking;

            return View();
        }


        // Save review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int BookingId,
            int Rating,
            string? Comment)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "User");
            }

            var booking =
                await _bookingService.GetBookingByIdAsync(BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // Make sure the booking belongs to the logged-in tourist
            if (booking.UserId != userId)
            {
                return Forbid();
            }

            // Reviews can only be submitted for approved bookings
            if (booking.Status != "Approved")
            {
                TempData["ReviewError"] =
                    "You can only review an approved booking.";

                return RedirectToAction("History", "Booking");
            }

            // Prevent duplicate reviews
            if (await _reviewService.HasReviewAsync(BookingId))
            {
                TempData["ReviewError"] =
                    "You have already submitted a review for this booking.";

                return RedirectToAction("History", "Booking");
            }

            // Validate rating
            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError(
                    "Rating",
                    "Rating must be between 1 and 5."
                );

                ViewBag.Booking = booking;

                return View();
            }

            var review = new Review
            {
                UserId = userId,
                BookingId = BookingId,
                Rating = Rating,
                Comment = Comment,
                CreatedAt = DateTime.Now
            };

            await _reviewService.AddReviewAsync(review);

            TempData["ReviewSuccess"] =
                "Thank you! Your review has been submitted successfully.";

            return RedirectToAction("History", "Booking");
        }
    }
}