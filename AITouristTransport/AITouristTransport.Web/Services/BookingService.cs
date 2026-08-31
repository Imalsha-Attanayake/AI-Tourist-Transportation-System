using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByProviderIdAsync(int providerId)
        {
            return await _bookingRepository.GetByProviderIdAsync(providerId);
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepository.UpdateAsync(booking);
        }
    }
}