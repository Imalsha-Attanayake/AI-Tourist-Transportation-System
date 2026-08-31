using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetReviewsByBookingIdAsync(int bookingId)
        {
            return await _reviewRepository.GetByBookingIdAsync(bookingId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _reviewRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> HasReviewAsync(int bookingId)
        {
            var reviews =
                await _reviewRepository.GetByBookingIdAsync(bookingId);

            return reviews.Any();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _reviewRepository.AddAsync(review);
        }
    }
}