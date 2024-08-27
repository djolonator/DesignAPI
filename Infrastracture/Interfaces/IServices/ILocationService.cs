

using Infrastracture.Models;

namespace Infrastracture.Interfaces.IServices
{
    public interface ILocationService
    {
        Task<List<LocationModel>> GetNearLocationsAsync(double latitude, double longitude, double radius);
    }
}
