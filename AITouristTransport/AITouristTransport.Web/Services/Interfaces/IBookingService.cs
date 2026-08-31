using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);

        Task<IEnumerable<Booking>> GetBookingsByProviderIdAsync(int providerId);

        Task<Booking?> GetBookingByIdAsync(int id);

        Task AddBookingAsync(Booking booking);

        Task UpdateBookingAsync(Booking booking);
    }
}