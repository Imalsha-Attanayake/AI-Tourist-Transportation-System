using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByBookingIdAsync(int bookingId);

        Task<IEnumerable<Review>> GetByUserIdAsync(int userId);

        Task AddAsync(Review review);
    }
}