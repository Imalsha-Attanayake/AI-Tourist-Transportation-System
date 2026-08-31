using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByProviderIdAsync(int providerId)
        {
            return await _vehicleRepository.GetByProviderIdAsync(providerId);
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(int id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }
    }
}