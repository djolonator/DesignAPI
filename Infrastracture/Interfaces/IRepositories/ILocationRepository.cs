
using Domain.Entities;

namespace Infrastracture.Interfaces.IRepositories
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetLocationsAsync();
        Task CreateLocationAsync(Location entity);
        Task DeleteLocationAsync(Location entity);
        Task<List<Location>> GetNearLocationsAsync(double latitude, double longitude, double radius);
    }
}
