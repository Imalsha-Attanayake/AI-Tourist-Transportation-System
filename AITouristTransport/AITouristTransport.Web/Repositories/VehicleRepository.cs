using Microsoft.EntityFrameworkCore;
using AITouristTransport.Web.Data;
using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;

namespace AITouristTransport.Web.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all vehicles
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // Get vehicles belonging to a specific provider
        public async Task<IEnumerable<Vehicle>> GetByProviderIdAsync(
            int providerId)
        {
            return await _context.Vehicles
                .Where(v => v.ProviderId == providerId)
                .ToListAsync();
        }

        // Get vehicle by ID
        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        // Add vehicle
        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        // Update vehicle
        public async Task UpdateAsync(Vehicle vehicle)
        {
            var existingVehicle =
                await _context.Vehicles.FindAsync(vehicle.VehicleId);

            if (existingVehicle != null)
            {
                existingVehicle.VehicleName = vehicle.VehicleName;
                existingVehicle.VehicleType = vehicle.VehicleType;
                existingVehicle.SeatCapacity = vehicle.SeatCapacity;
                existingVehicle.PricePerDay = vehicle.PricePerDay;
                existingVehicle.ImageUrl = vehicle.ImageUrl;

                // Keep the original ProviderId
                existingVehicle.ProviderId = vehicle.ProviderId;

                await _context.SaveChangesAsync();
            }
        }

        // Delete vehicle
        public async Task DeleteAsync(int id)
        {
            var vehicle =
                await _context.Vehicles.FindAsync(id);

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);

                await _context.SaveChangesAsync();
            }
        }
    }
}