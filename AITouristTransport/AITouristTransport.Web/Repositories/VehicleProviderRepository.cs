using Microsoft.EntityFrameworkCore;
using AITouristTransport.Web.Data;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;

namespace AITouristTransport.Web.Repositories
{
    public class VehicleProviderRepository : IVehicleProviderRepository
    {
        private readonly AppDbContext _context;

        public VehicleProviderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleProvider>> GetAllAsync()
        {
            return await _context.VehicleProviders
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<VehicleProvider?> GetByIdAsync(int id)
        {
            return await _context.VehicleProviders
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProviderId == id);
        }

        public async Task<VehicleProvider?> GetByUserIdAsync(int userId)
        {
            return await _context.VehicleProviders
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(VehicleProvider provider)
        {
            await _context.VehicleProviders.AddAsync(provider);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleProvider provider)
        {
            _context.VehicleProviders.Update(provider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var provider = await _context.VehicleProviders.FindAsync(id);

            if (provider != null)
            {
                _context.VehicleProviders.Remove(provider);
                await _context.SaveChangesAsync();
            }
        }
    }
}