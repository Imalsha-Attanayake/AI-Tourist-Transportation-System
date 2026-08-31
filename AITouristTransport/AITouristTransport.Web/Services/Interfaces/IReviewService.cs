using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByBookingIdAsync(int bookingId);

        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);

        Task<bool> HasReviewAsync(int bookingId);

        Task AddReviewAsync(Review review);
    }
}