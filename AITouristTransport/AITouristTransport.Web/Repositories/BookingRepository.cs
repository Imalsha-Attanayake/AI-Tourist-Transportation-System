using AITouristTransport.Web.Data;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AITouristTransport.Web.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Provider)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Provider)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByProviderIdAsync(int providerId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Where(b => b.ProviderId == providerId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Provider)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}