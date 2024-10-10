using Infrastracture.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors("AllowLocalhost3000")]
    public class PostersController : ControllerBase
    {
        private readonly IDesignService _designService;

        public PostersController(IDesignService designService)
        {
            _designService = designService;
        }

        //[Authorize]
        [HttpGet("designsSearch")]
        public async Task<IActionResult> DesignsSearch([FromQuery] string term)
        {
            var result = await _designService.SearchDesigns(term);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        //[Authorize]
        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            var result = await _designService.GetDesignCategoriesAsync();

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        //[Authorize]
        [HttpGet("design/{designId}")]
        public async Task<IActionResult> DesignById(int designId)
        {
            var result = await _designService.GetDesignByIdAsync(designId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        //[Authorize]
        [HttpGet("designsByCategory/{categoryId}")]
        public async Task<IActionResult> DesignsByCategory([FromRoute]int categoryId, [FromQuery] int page)
        {
            int pageSize = 5;
            var result = await _designService.GetGesignsByCategoryIdPaginated(categoryId, pageSize, page);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

    }
}
