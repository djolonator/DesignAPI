using Application.Services;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using Microsoft.AspNetCore.Cors;
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
        public ParkingLotController(ILocationService locationService)
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
            double latitude = 44.787197, longitude = 20.457273, radius = 0.00000007848061;
            var locations = await _locationService.GetNearLocationsAsync(latitude, longitude, radius);
            return Ok();
        }
    }
}
