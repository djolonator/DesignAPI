using Application.Models;
using Application.Repositories;
using AutoMapper;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<LocationModel>> GetNearLocationsAsync()
        {
            double latitude = 0, longitude = 0, radius = 0;
            var nearLocations = _locationRepository.GetNearLocationsAsync(latitude, longitude, radius);
            var result = _mapper.Map<List<LocationModel>>(nearLocations);
            return result;
        }
    }
}
