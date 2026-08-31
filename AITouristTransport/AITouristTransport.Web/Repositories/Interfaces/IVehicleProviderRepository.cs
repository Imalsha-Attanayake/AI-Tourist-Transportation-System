using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Repositories.Interfaces
{
    public interface IVehicleProviderRepository
    {
        Task<IEnumerable<VehicleProvider>> GetAllAsync();
        Task<VehicleProvider?> GetByIdAsync(int id);
        Task<VehicleProvider?> GetByUserIdAsync(int userId);
        Task AddAsync(VehicleProvider provider);
        Task UpdateAsync(VehicleProvider provider);
        Task DeleteAsync(int id);
    }
}