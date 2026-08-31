using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();

        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);

        Task<IEnumerable<Booking>> GetByProviderIdAsync(int providerId);

        Task<Booking?> GetByIdAsync(int id);

        Task AddAsync(Booking booking);

        Task UpdateAsync(Booking booking);
    }
}