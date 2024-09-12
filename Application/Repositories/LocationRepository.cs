using Domain;
using Domain.Entities;
using Infrastracture.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class LocationRepository: ILocationRepository
    {
        private readonly StorageDbContext _parkingDbContext;
        public LocationRepository(StorageDbContext parkingDbContext)
        {
            _parkingDbContext = parkingDbContext;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            return await _parkingDbContext.Locations.ToListAsync();
        }

        public async Task CreateLocationAsync(Location entity)
        {
            await _parkingDbContext.Locations.AddAsync(entity);
            await _parkingDbContext.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(Location entity)
        {
            _parkingDbContext.Locations.Remove(entity);
            await _parkingDbContext.SaveChangesAsync();
        }
        public async Task<List<Location>> GetNearLocationsAsync(double latitude, double longitude, double radius)
        {
            var query = from record in _parkingDbContext.Locations
                        let absDiffLat = Math.Abs(record.Latitude - latitude)
                        let absDiffLong = Math.Abs(record.Longitude - longitude)
                        where absDiffLat <= radius && absDiffLong <= radius
                        select record;

            var nearLocations = await query.ToListAsync();

            return nearLocations;
        }

    }
}
