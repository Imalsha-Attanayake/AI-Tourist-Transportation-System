using AITouristTransport.Web.Data;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AITouristTransport.Web.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Booking)
                .Where(r => r.BookingId == bookingId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Booking)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
    }
}