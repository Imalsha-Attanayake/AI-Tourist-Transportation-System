using AITouristTransport.Web.Models;

namespace AITouristTransport.Web.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        Task<IEnumerable<Vehicle>> GetVehiclesByProviderIdAsync(int providerId);

        Task<Vehicle?> GetVehicleByIdAsync(int id);

        Task AddVehicleAsync(Vehicle vehicle);

        Task UpdateVehicleAsync(Vehicle vehicle);

        Task DeleteVehicleAsync(int id);
    }
}