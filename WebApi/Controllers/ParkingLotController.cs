using Application.Models;
using Application.Services;
using Domain.Entities;
using Infrastracture.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [EnableCors("AllowLocalhost3001")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public ParkingLotController(LocationService locationService)
        {
            _locationService = locationService;
        }


        [HttpPost]
        public async Task<IActionResult> PostLocation(LocationModel location)
        {
            return Ok(location);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            return Ok();
        }


        static List<LocationModel> GenerateRandomLocations()
        {
            const double fiveHundreedMRadius = 0.00000007848061;

            Random random = new Random();
            int count = 20;
            const double centralLat = 44.787197;
            const double centralLong = 20.457273;


            const double latRange = 0.1; 
            const double longRange = 0.2; 

            LocationModel[] locations = new LocationModel[count];

            for (int i = 0; i < count; i++)
            {
                double lat = centralLat + ((random.NextDouble() - 0.5) * latRange);
                double longi = centralLong + ((random.NextDouble() - 0.5) * longRange);
                locations[i] = new LocationModel() { Lat = lat, Lng = longi};
            }

            return locations.ToList();
        }

    }
}
