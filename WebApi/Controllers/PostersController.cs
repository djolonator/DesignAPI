using FluentValidation;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Route("api")]
    [ApiController]
    [EnableCors("AllowLocalhost3000")]
    public class PostersController : ControllerBase
    {
        private readonly IDesignService _designService;
        private readonly ICheckoutService _checkoutService;
        private readonly IValidator<CheckoutRequest> _validator;

        public PostersController(IDesignService designService, ICheckoutService checkoutService, IValidator<CheckoutRequest> validator)
        {
            _designService = designService;
            _checkoutService = checkoutService;
            _validator = validator;
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

        [Authorize]
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

        [Authorize]
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
        [HttpPost("initiatePaypallOrder")]
        public async Task<ActionResult> InitiatePaypallOrder()
        {
            var userId = GetLoggedInUserId();
            var result = await _checkoutService.HandleInitiatePaypallOrder(userId);
            return Ok(result);
        }

        //[Authorize]
        [HttpPost("capturePaypallOrder/{paypallOrderId}")]
        public async Task<ActionResult> CapturePaypallOrder([FromRoute] string paypallOrderId)
        {
            var result = await _checkoutService.HandleCapturePaypallOrder(paypallOrderId);
            return Ok(result);
        }

        //[Authorize]
        [HttpPost("calculateCost")]
        public async Task<ActionResult> CalculateCost([FromBody] CheckoutRequest checkout)
        {
            var validationResult = await _validator.ValidateAsync(checkout);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userId = GetLoggedInUserId();
            var result = await _checkoutService.CalculateTotalCost(checkout, userId);
            return Ok(result);
        }

        private string GetLoggedInUserId()
        {
            var userClaims = User.Claims.ToList();
            //return User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
            return "79caf87d-631f-456a-a1b9-0289b2b14b82";
        }
    }
}
