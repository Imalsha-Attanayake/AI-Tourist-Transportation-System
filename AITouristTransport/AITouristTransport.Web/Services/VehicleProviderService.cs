using AITouristTransport.Web.Models;
using AITouristTransport.Web.Repositories.Interfaces;
using AITouristTransport.Web.Services.Interfaces;

namespace AITouristTransport.Web.Services
{
    public class VehicleProviderService : IVehicleProviderService
    {
        private readonly IVehicleProviderRepository _vehicleProviderRepository;

        public VehicleProviderService(
            IVehicleProviderRepository vehicleProviderRepository)
        {
            _vehicleProviderRepository = vehicleProviderRepository;
        }

        public async Task<IEnumerable<VehicleProvider>> GetAllAsync()
        {
            return await _vehicleProviderRepository.GetAllAsync();
        }

        public async Task<VehicleProvider?> GetByIdAsync(int id)
        {
            return await _vehicleProviderRepository.GetByIdAsync(id);
        }

        public async Task<VehicleProvider?> GetByUserIdAsync(int userId)
        {
            return await _vehicleProviderRepository.GetByUserIdAsync(userId);
        }

        public async Task AddAsync(VehicleProvider provider)
        {
            await _vehicleProviderRepository.AddAsync(provider);
        }

        public async Task UpdateAsync(VehicleProvider provider)
        {
            await _vehicleProviderRepository.UpdateAsync(provider);
        }

        public async Task DeleteAsync(int id)
        {
            await _vehicleProviderRepository.DeleteAsync(id);
        }
    }
}