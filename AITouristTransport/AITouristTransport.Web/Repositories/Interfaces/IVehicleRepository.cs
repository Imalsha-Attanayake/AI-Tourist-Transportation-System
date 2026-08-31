using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();

        Task<IEnumerable<Vehicle>> GetByProviderIdAsync(int providerId);

        Task<Vehicle?> GetByIdAsync(int id);

        Task AddAsync(Vehicle vehicle);

        Task UpdateAsync(Vehicle vehicle);

        Task DeleteAsync(int id);
    }
}