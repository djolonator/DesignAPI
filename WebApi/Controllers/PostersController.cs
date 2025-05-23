﻿using FluentValidation;
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
        private readonly IPosterService _posterService;
        private readonly ICheckoutService _checkoutService;
        private readonly IOrderService _orderService;
        private readonly IValidator<CheckoutRequest> _validator;

        public PostersController(IPosterService posterService, ICheckoutService checkoutService, IValidator<CheckoutRequest> validator, IOrderService orderService)
        {
            _posterService = posterService;
            _checkoutService = checkoutService;
            _validator = validator;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("designsSearch")]
        public async Task<IActionResult> DesignsSearch([FromQuery] string term, [FromQuery] int page)
        {
            int pageSize = 5;
            var result = await _posterService.SearchDesigns(term, pageSize, page);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            
            var result = await _posterService.GetDesignCategoriesAsync();

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpGet("design/{designId}")]
        public async Task<IActionResult> DesignById([FromRoute] int designId)
        {
            var result = await _posterService.GetDesignByIdAsync(designId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpGet("designsByCategory/{categoryId}")]
        public async Task<IActionResult> DesignsByCategory([FromRoute]int categoryId, [FromQuery] int page)
        {
            int pageSize = 5;
            var result = await _posterService.GetGesignsByCategoryIdPaginated(categoryId, pageSize, page);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpGet("bestsellingDesigns")]
        public async Task<IActionResult> BestsellingDesigns([FromQuery] int page)
        {
            int pageSize = 5;
            var result = await _posterService.GetBestsellingDesignsPaginated(pageSize, page);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpPost("initiatePaypallOrder")]
        public async Task<IActionResult> InitiatePaypallOrder()
        {
            var userId = GetLoggedInUserId();
            var result = await _checkoutService.HandleInitiatePaypallOrder(userId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpPost("capturePaypallOrder/{paypallOrderId}")]
        public async Task<IActionResult> CapturePaypallOrder([FromRoute] string paypallOrderId)
        {
            var userId = GetLoggedInUserId();
            var result = await _checkoutService.HandleCapturePaypallOrder(paypallOrderId, userId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpPost("estimateTotalCost")]
        public async Task<IActionResult> EstimateTotalCost([FromBody] CheckoutRequest checkout)
        {
            var validationResult = await _validator.ValidateAsync(checkout);

            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.Errors);
            }
            var userId = GetLoggedInUserId();
            var result = await _checkoutService.EstimateTotalCost(checkout, userId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        [Authorize]
        [HttpGet("orders")]
        public async Task<IActionResult> Orders()
        {
            var userId = GetLoggedInUserId();
            var result = await _orderService.GetOrdersForUser(userId);

            return result.Map<IActionResult>(
                onSuccess: result => Ok(result),
                onFailure: error => BadRequest(error));
        }

        private string GetLoggedInUserId()
        {
            var userClaims = User.Claims.ToList();
            return User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
        }
    }
}
