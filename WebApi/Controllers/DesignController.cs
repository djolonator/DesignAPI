using Infrastracture.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class DesignController : ControllerBase
    {
        private readonly IDesignService _designService;

        public DesignController(IDesignService designService)
        {
            _designService = designService;
        }

        [Authorize]
        [HttpGet("designs")]
        public async Task<IActionResult> Designs([FromQuery] string term)
        {
            var result = await _designService.SearchDesigns(term);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
