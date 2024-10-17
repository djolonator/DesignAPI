using Infrastracture.Interfaces.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors("AllowLocalhost3000")]
    public class PostersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDesignService _designService;

        public PostersController(IDesignService designService, IHttpClientFactory httpClientFactory)
        {
            _designService = designService;
            _httpClientFactory = httpClientFactory;
        }

        //[Authorize]
        [HttpGet("designsSearch")]
        public async Task<IActionResult> DesignsSearch([FromQuery] string term)
        {
            var result = await _designService.SearchDesigns(term);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        //[Authorize]
        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            var result = await _designService.GetDesignCategoriesAsync();

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        //[Authorize]
        [HttpGet("design/{designId}")]
        public async Task<IActionResult> DesignById([FromRoute] int designId)
        {
            var result = await _designService.GetDesignByIdAsync(designId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        //[Authorize]
        [HttpGet("designsByCategory/{categoryId}")]
        public async Task<IActionResult> DesignsByCategory([FromRoute]int categoryId, [FromQuery] int page)
        {
            int pageSize = 5;
            var result = await _designService.GetGesignsByCategoryIdPaginated(categoryId, pageSize, page);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://api.github.com");
            string result = await client.GetStringAsync("/");
            return Ok(result);
        }
    }
}
