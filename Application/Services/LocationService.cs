using Application.Repositories;
using AutoMapper;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;


namespace Application.Services
{
    public class LocationService: ILocationService
    {
        private readonly IMapper _mapper;
        private readonly ILocationRepository _locationRepository;
        public LocationService(IMapper mapper, LocationRepository locationRepository)
        {
            _mapper = mapper;
            _locationRepository = locationRepository;
        }

        public async Task<List<LocationModel>> GetNearLocationsAsync(double latitude, double longitude, double radius)
        {
            var nearLocations = await _locationRepository.GetNearLocationsAsync(latitude, longitude, radius);
            var result = _mapper.Map<List<LocationModel>>(nearLocations);
            return result;
        }
    }
}
